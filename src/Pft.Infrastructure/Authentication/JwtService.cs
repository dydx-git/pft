using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Pft.Application.Abstractions.Authentication;
using Pft.Domain.Abstractions;
using Pft.Infrastructure.Authentication.Models;

namespace Pft.Infrastructure.Authentication;
internal sealed class JwtService(HttpClient httpClient, IOptions<KeycloakOptions> keycloakOptions)
    : IJwtService
{
    private static readonly Error AuthenticationFailed = new("Keycloak.AuthenticationFailed",
        "Failed to acquire access token to do authentication failure");

    private readonly KeycloakOptions _keycloakOptions = keycloakOptions.Value;

    public async Task<Result<string>> GetAccessTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            var authRequestParameters = new KeyValuePair<string, string>[]
            {
                new("client_id", _keycloakOptions.AuthClientId),
                new("client_secret", _keycloakOptions.AuthClientSecret),
                new("scope", "openid email"),
                new("grant_type", "password"),
                new("username", email),
                new("password", password)
            };

            var authorizationRequestContent = new FormUrlEncodedContent(authRequestParameters);

            var response = await httpClient.PostAsync("", authorizationRequestContent, cancellationToken);
            response.EnsureSuccessStatusCode();

            var authorizationToken = await response.Content.ReadFromJsonAsync<AuthorizationToken>();
            if (authorizationToken is null) return Result.Failure<string>(AuthenticationFailed);

            return authorizationToken.AccessToken;
        }
        catch (HttpRequestException)
        {
            return Result.Failure<string>(AuthenticationFailed);
        }
    }
}
