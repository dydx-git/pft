using Asp.Versioning;
using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pft.Application.Abstractions.Authentication;
using Pft.Application.Abstractions.Caching;
using Pft.Application.Abstractions.Clock;
using Pft.Application.Abstractions.Data;
using Pft.Application.Abstractions.Email;
using Pft.Domain.Abstractions;
using Pft.Domain.Entities.Accounts;
using Pft.Domain.Entities.Transactions;
using Pft.Domain.Entities.Users;
using Pft.Infrastructure.Authentication;
using Pft.Infrastructure.Authorization;
using Pft.Infrastructure.Caching;
using Pft.Infrastructure.Clock;
using Pft.Infrastructure.Data;
using Pft.Infrastructure.Email;
using Pft.Infrastructure.Outbox;
using Pft.Infrastructure.Repositories;
using Quartz;
using IAuthenticationService = Pft.Application.Abstractions.Authentication.IAuthenticationService;

namespace Pft.Infrastructure;

using AuthenticationOptions = Authentication.AuthenticationOptions;
using AuthenticationService = Authentication.AuthenticationService;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IEmailService, EmailService>();

        AddPersistence(services, configuration);

        AddAuthentication(services, configuration);

        AddBackgroundJobs(services, configuration);

        AddAuthorization(services);

        AddCaching(services, configuration);

        AddHealthChecks(services, configuration);

        AddApiVersioning(services);

        return services;
    }

    private static void AddBackgroundJobs(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OutboxOptions>(configuration.GetSection("Outbox"));

        services.AddQuartz(configurator =>
        {
            var schedulerId = Guid.NewGuid();
            configurator.SchedulerId = $"default-id-{schedulerId}";
            configurator.SchedulerName = $"default-name-{schedulerId}";
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        services.ConfigureOptions<ProcessOutboxMessageJobSetup>();
    }

    private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication"));

        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services.Configure<KeycloakOptions>(configuration.GetSection("Keycloak"));

        services.AddTransient<AdminAuthorizationDelegatingHandler>();

        services.AddHttpClient<IAuthenticationService, AuthenticationService>((serviceProvider, httpclient) =>
        {
            var keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;

            httpclient.BaseAddress = new Uri(keycloakOptions.AdminUrl);
        }).AddHttpMessageHandler<AdminAuthorizationDelegatingHandler>();

        services.AddHttpClient<IJwtService, JwtService>((serviceProvider, httpclient) =>
        {
            var keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;

            httpclient.BaseAddress = new Uri(keycloakOptions.TokenUrl);
        });

        services.AddHttpContextAccessor();

        services.AddScoped<IUserContext, UserContext>();
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database") ??
            throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
        });

        #region Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        #endregion

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));

        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
    }

    private static void AddAuthorization(IServiceCollection services)
    {
        services.AddScoped<AuthorizationService>();

        services.AddTransient<IClaimsTransformation, CustomClaimsTransformation>();

        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
    }
    
    private static void AddCaching(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = GetConnectionString(configuration, "Cache");

        services.AddStackExchangeRedisCache(options => options.Configuration = connectionString);

        services.AddSingleton<ICacheService, CacheService>();
    }

    private static string GetConnectionString(IConfiguration configuration, string connectionName) =>
        configuration.GetConnectionString(connectionName) ??
        throw new ArgumentNullException(nameof(configuration));

    private static void AddHealthChecks(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddNpgSql(GetConnectionString(configuration, "Database"))
            .AddRedis(GetConnectionString(configuration, "Cache"))
            .AddUrlGroup(new Uri(configuration["KeyCloak:BaseUrl"] 
                                 ?? throw new ArgumentNullException(Error.NullValue.ToString())), HttpMethod.Get, "keycloak");
    }


    ///<summary>
    /// Add API Versioning when using Controllers
    /// If using Minimal APIs, consider the Nuget Package "Asp.Versioning.Http"
    /// </summary>
    /// <param name="services"></param>
    private static void AddApiVersioning(IServiceCollection services)
    {
        services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
    }
}
