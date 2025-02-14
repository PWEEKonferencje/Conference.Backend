using System.Reflection;
using System.Text.Json.Serialization;
using Application;
using Domain;
using Domain.Entities;
using Domain.Shared;
using Infrastructure;
using Infrastructure.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using WebApi;

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
builder.Services.AddHttpContextAccessor();
builder.Services.AddIdentity<Identity, IdentityRole>()
	.AddEntityFrameworkStores<ConferenceDbContext>();
builder.Services.AddControllers(options =>
	{
		options.Filters.Add(new AuthorizeFilter( new AuthorizationPolicyBuilder()
			.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build()));
		options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorResult), 500));
		options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorResult), 400));
		options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
	})
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
		options.JsonSerializerOptions.WriteIndented = true;
		options.JsonSerializerOptions.AllowTrailingCommas = true;
	});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
	option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please enter a valid token",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		BearerFormat = "JWT",
		Scheme = "Bearer"
	});
	option.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new List<string>()
		}
	});
});
//builder.Services.AddTransient<LoggingMiddleware>();


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
//app.UseMiddleware<LoggingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program {}