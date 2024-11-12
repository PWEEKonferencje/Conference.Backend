using FluentAssertions;
using FluentValidation;
using MediatR;
using NetArchTest.Rules;

namespace ArchitectureTests;

public class ApplicationTests : BaseArchitectureTest
{
	[Fact]
	public void Handler_ShouldHave_NameEndingWith_Handler()
	{
		var result = Types.InAssembly(ApplicationAssembly)
			.That()
			.ImplementInterface(typeof(IRequestHandler<>))
			.Should()
			.HaveNameEndingWith("Handler")
			.GetResult();
		result.IsSuccessful.Should().BeTrue();
	}
	
	[Fact]
	public void Handler_Should_NotBePublic()
	{
		var result = Types.InAssembly(ApplicationAssembly)
			.That()
			.ImplementInterface(typeof(IRequestHandler<>))
			.Should()
			.NotBePublic()
			.GetResult();
		result.IsSuccessful.Should().BeTrue();
	}
	
	[Fact]
	public void Handlers_Should_OnlyBeInApplication()
	{
		var result = Types.InAssembly(ApplicationAssembly)
			.That()
			.ImplementInterface(typeof(IRequestHandler<>))
			.Should()
			.ResideInNamespace("Application")
			.GetResult();
		result.IsSuccessful.Should().BeTrue();
	}
	
	[Fact]
	public void Validators_Should_OnlyBeInApplication()
	{
		var result = Types.InAssembly(ApplicationAssembly)
			.That()
			.ImplementInterface(typeof(IValidator<>))
			.Should()
			.ResideInNamespace("Application")
			.GetResult();
		result.IsSuccessful.Should().BeTrue();
	}
	
	[Fact]
	public void MappingProfiles_Should_OnlyBeInApplication()
	{
		var result = Types.InAssembly(ApplicationAssembly)
			.That()
			.Inherit(typeof(AutoMapper.Profile))
			.Should()
			.ResideInNamespace("Application")
			.GetResult();
		result.IsSuccessful.Should().BeTrue();
	}
}