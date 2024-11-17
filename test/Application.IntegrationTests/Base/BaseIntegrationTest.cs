using Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;

namespace IntegrationTests.Base;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>, IAsyncLifetime
{
	private readonly IServiceScope _scope;
	protected readonly IServiceProvider ServiceProvider;
	protected readonly IMediator Mediator;
	protected readonly ConferenceDbContext DbContext;
	protected readonly IHttpContextAccessor HttpContextAccessor;
	private Respawner _respawner;
	private readonly string _connectionString;

	protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
	{
		_scope = factory.Services.CreateScope();
		ServiceProvider = _scope.ServiceProvider;
		Mediator = ServiceProvider.GetRequiredService<IMediator>();
		DbContext = ServiceProvider.GetRequiredService<ConferenceDbContext>();
		HttpContextAccessor = ServiceProvider.GetRequiredService<IHttpContextAccessor>();
		_connectionString = factory.DbContainer.GetConnectionString();
	}

	public async Task InitializeAsync()
	{
		await using var conn = new NpgsqlConnection(_connectionString);
		await conn.OpenAsync();
		_respawner = await Respawner.CreateAsync(conn, new RespawnerOptions
		{
			DbAdapter = DbAdapter.Postgres
		});
	}

	public async Task DisposeAsync()
	{
		await using var conn = new NpgsqlConnection(_connectionString);
		await conn.OpenAsync();
		await _respawner.ResetAsync(conn);
	}
}