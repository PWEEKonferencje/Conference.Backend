using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjectionExtension
{
	/// <summary>
	/// Method used to register application services in DI container
	/// </summary>
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		// Add application services here
		services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjectionExtension).Assembly));
		services.AddValidatorsFromAssembly(typeof(DependencyInjectionExtension).Assembly);
		return services;
	}
}