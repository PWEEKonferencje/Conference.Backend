using Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace IntegrationTests.Base;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
	public readonly PostgreSqlContainer DbContainer
		= new PostgreSqlBuilder()
			.WithImage("postgres:latest")
			.WithDatabase("ConferenceTest")
			.WithUsername("devtest")
			.WithPassword("DevPassword123")
			.Build();
	
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		var connectionString = DbContainer.GetConnectionString();
		builder.ConfigureTestServices(services =>
		{
			var descriptor = services
				.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<ConferenceDbContext>));
			if (descriptor is not null)
				services.Remove(descriptor);
			
			services.AddDbContext<ConferenceDbContext>(options =>
				options.UseNpgsql(DbContainer.GetConnectionString()));
		});
	}

	public async Task InitializeAsync()
	{
		await DbContainer.StartAsync();
		using var scope = Services.CreateScope();
		var context = scope.ServiceProvider.GetRequiredService<ConferenceDbContext>();
		await context.Database.MigrateAsync();
	}

	public new Task DisposeAsync()
	{
		return DbContainer.StopAsync();
	}
}