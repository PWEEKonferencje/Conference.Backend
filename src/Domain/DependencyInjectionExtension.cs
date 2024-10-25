using Microsoft.Extensions.DependencyInjection;

namespace Domain;

public static class DependencyInjectionExtension
{
	/// <summary>
	/// Method used to register domain services in DI container
	/// </summary>
	public static IServiceCollection AddDomain(this IServiceCollection services)
	{
		// Add domain services here
		return services;
	}
}