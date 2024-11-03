using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Application;
using Domain;
using Domain.Entities.Identity;
using Infrastructure;
using Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Serilog.Events;
using WebApi;
using WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.AddUserSecrets<GlobalExceptionHandler>()
	.AddEnvironmentVariables();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddDomain();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddIdentity<UserAccount, IdentityRole>()
	.AddEntityFrameworkStores<ConferenceDbContext>()
	.AddDefaultTokenProviders();

builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
		options.JsonSerializerOptions.WriteIndented = true;
		options.JsonSerializerOptions.AllowTrailingCommas = true;
	});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Log.Logger = new LoggerConfiguration()
	.WriteTo.Debug(restrictedToMinimumLevel: LogEventLevel.Debug)
	.WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
	.Enrich.FromLogContext()
	.CreateLogger();
builder.Host.UseSerilog();

var app = builder.Build();

app.Lifetime.ApplicationStarted.Register(() =>
{
	var serverAddresses = app.Urls;
	foreach (var i in serverAddresses)
	{
		Log.Information("Swagger running at {Url}/swagger/index.html", i);
	}	
});

app.UseExceptionHandler(_ => {});
app.UseMiddleware<LoggingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();