using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NetArchTest.Rules;

namespace ArchitectureTests;

public class WebApiTests : BaseArchitectureTest
{
	[Fact]
	public void Controllers_Should_OnlyBeInWebApi()
	{
		var result = Types.InAssembly(WebApiAssembly)
			.That()
			.Inherit(typeof(ControllerBase))
			.Should()
			.ResideInNamespace("WebApi.Controllers")
			.And()
			.HaveCustomAttribute(typeof(ApiControllerAttribute))
			.GetResult();
		result.IsSuccessful.Should().BeTrue();
	}
	
	[Fact]
	public void Controllers_Should_HaveNameEndingWithController()
	{
		var result = Types.InAssembly(WebApiAssembly)
			.That()
			.Inherit(typeof(ControllerBase))
			.Should()
			.HaveNameEndingWith("Controller")
			.GetResult();
		result.IsSuccessful.Should().BeTrue();
	}

	[Fact]
	public void Controllers_Should_NotInjectServices()
	{
		var types = Types.InAssembly(WebApiAssembly)
			.That()
			.Inherit(typeof(ControllerBase))
			.GetTypes();
		foreach (var controller in types)
		{
			var ctors = controller.GetConstructors();
			
			foreach (var constructor in ctors)
			{
				var parameterCount = constructor.GetParameters().Length;
				Assert.True(parameterCount <= 1,
					$"{controller.Name} has a constructor with {parameterCount} parameters, exceeding the maximum of {1}.");
			}
		}
	}
	
	[Fact]
	public void Middlewares_Should_OnlyBeInWebApi()
	{
		var result = Types.InAssembly(WebApiAssembly)
			.That()
			.Inherit(typeof(Microsoft.AspNetCore.Http.IMiddleware))
			.Should()
			.ResideInNamespace("WebApi.Middlewares")
			.GetResult();
		result.IsSuccessful.Should().BeTrue();
	}
	
	[Fact]
	public void Middlewares_Should_HaveNameEndingWithMiddleware()
	{
		var result = Types.InAssembly(WebApiAssembly)
			.That()
			.Inherit(typeof(Microsoft.AspNetCore.Http.IMiddleware))
			.Should()
			.HaveNameEndingWith("Middleware")
			.GetResult();
		result.IsSuccessful.Should().BeTrue();
	}
}