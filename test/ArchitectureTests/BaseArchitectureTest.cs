using System.Reflection;

namespace ArchitectureTests;

public abstract class BaseArchitectureTest
{
	protected static readonly Assembly WebApiAssembly = typeof(WebApi.GlobalExceptionHandler).Assembly;
	protected static readonly Assembly ApplicationAssembly = typeof(Application.DependencyInjectionExtension).Assembly;
	protected static readonly Assembly InfrastructureAssembly = typeof(Infrastructure.DependencyInjectionExtension).Assembly;
	protected static readonly Assembly DomainAssembly = typeof(Domain.DependencyInjectionExtension).Assembly;
}