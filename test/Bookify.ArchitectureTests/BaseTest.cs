using System.Reflection;
using Pft.Application.Abstractions.Messaging;
using Pft.Domain.Abstractions;
using Pft.Infrastructure;

namespace Bookify.ArchitectureTests;

public abstract class BaseTest
{
    protected static Assembly ApplicationAssembly => typeof(IBaseCommand).Assembly;

    protected static Assembly DomainAssembly => typeof(IEntity).Assembly;

    protected static Assembly InfrastructureAssembly => typeof(ApplicationDbContext).Assembly;
}