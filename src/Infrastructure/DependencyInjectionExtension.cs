using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjectionExtension
{
	/// <summary>
	/// Method used to register infrastructure services in DI container
	/// </summary>
	public static IServiceCollection AddInfrastructure(this IServiceCollection services)
	{
		// Add infrastructure services here
		return services;
	}
}