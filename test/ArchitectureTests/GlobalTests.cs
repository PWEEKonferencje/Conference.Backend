using FluentAssertions;
using NetArchTest.Rules;

namespace ArchitectureTests;

public class GlobalTests : BaseArchitectureTest
{
	[Fact]
	public void DomainLayer_Should_NotHaveDependencyOnApplication()
	{
		var result = Types.InAssembly(DomainAssembly)
			.Should()
			.NotHaveDependencyOn(ApplicationAssembly.GetName().Name)
			.GetResult();
		result.IsSuccessful.Should().BeTrue();
	}
    
	[Fact]
	public void DomainLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
	{
		var result = Types.InAssembly(DomainAssembly)
			.Should()
			.NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
			.GetResult();
		result.IsSuccessful.Should().BeTrue();
	}
    
	[Fact]
	public void ApplicationLayer_ShouldNotHaveDependencyOn_WebApiLayer()
	{
		var result = Types.InAssembly(ApplicationAssembly)
			.Should()
			.NotHaveDependencyOn(WebApiAssembly.GetName().Name)
			.GetResult();
		result.IsSuccessful.Should().BeTrue();
	}
    
	[Fact]
	public void InfrastructureLayer_ShouldNotHaveDependencyOn_WebApiLayer()
	{
		var result = Types.InAssembly(InfrastructureAssembly)
			.Should()
			.NotHaveDependencyOn(WebApiAssembly.GetName().Name)
			.GetResult();
		result.IsSuccessful.Should().BeTrue();
	}
}