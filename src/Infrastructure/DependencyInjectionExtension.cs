using System.Text;
using Application.Common.Configuration;
using Application.Common.Services;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Authentication;
using Infrastructure.Database;
using Infrastructure.Dictionaries.UniversityNames;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static class DependencyInjectionExtension
{
	/// <summary>
	/// Method used to register infrastructure services in DI container
	/// </summary>
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		// Add infrastructure services here
		services.AddDbContext<ConferenceDbContext>(options =>
			options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
		
        #region Authentication
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        var authenticationConfiguration =
	        configuration.GetSection("Authentication").Get<AuthenticationConfiguration>() ??
	        new AuthenticationConfiguration();
        services.AddSingleton(authenticationConfiguration);
        services.AddAuthentication(options =>
	        {
		        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	        })
	        .AddJwtBearer(options =>
	        {
		        options.TokenValidationParameters = new TokenValidationParameters
		        {
			        ValidateIssuer = true,
			        ValidateAudience = true,
			        ValidateLifetime = true,
			        ValidateIssuerSigningKey = true,
			        ValidIssuer =
				        authenticationConfiguration.JwtIssuer ??
				        throw new KeyNotFoundException("JWT Issuer environmental variable is missing"),
			        ValidAudience =
				        authenticationConfiguration.JwtAudience ??
				        throw new KeyNotFoundException("JWT Audience environmental variable is missing"),
			        IssuerSigningKey =
				        new SymmetricSecurityKey(
					        Encoding.UTF8.GetBytes(
						        authenticationConfiguration.JwtKey ??
						        throw new KeyNotFoundException("JWT key environmental variable is missing")))
		        };
	        })
	        .AddGitHub(githubOptions =>
	        {
		        githubOptions.ClientId
			        = configuration["OAuth2:GitHub:ClientId"] ??
			          throw new KeyNotFoundException("GitHub ClientId environmental variable is missing");
		        githubOptions.ClientSecret
			        = configuration["OAuth2:GitHub:ClientSecret"] ??
			          throw new KeyNotFoundException("GitHub ClientSecret environmental variable is missing");
		        githubOptions.Scope.Add("user:email");
		        githubOptions.SaveTokens = true;
		        githubOptions.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
		        githubOptions.TokenEndpoint = "https://github.com/login/oauth/access_token";
		        githubOptions.UserInformationEndpoint = "https://api.github.com/user";
	        });
        services.AddScoped<IUserContextService, UserContextService>();
        services.AddHttpContextAccessor();
        #endregion
        
        services.AddSingleton<IUniversityNameRepository>(_ => new UniversityNameRepository());
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<IIdentityRepository, IdentityRepository>();
        services.AddScoped<IAffiliationRepository, AffiliationRepository>();
        services.AddScoped<IConferenceRepository, ConferenceRepository>();
        services.AddScoped<IAttendeeRepository, AttendeeRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
		return services;
	}
}