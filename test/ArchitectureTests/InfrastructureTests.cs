using FluentAssertions;
using Microsoft.EntityFrameworkCore.Migrations;
using NetArchTest.Rules;

namespace ArchitectureTests;

public class InfrastructureTests : BaseArchitectureTest
{
	[Fact]
	public void IRepositoryImplementations_Should_OnlyBeInInfrastructure()
	{
		var result = Types.InAssembly(InfrastructureAssembly)
			.That()
			.AreClasses()
			.And()
			.ImplementInterface(typeof(Domain.Repositories.IRepository<>))
			.Should()
			.ResideInNamespace("Infrastructure.Repositories")
			.GetResult();
		result.IsSuccessful.Should().BeTrue();
	}

	[Fact]
	public void Migrations_Should_OnlyBeInInfrastructure()
	{
		var result = Types.InAssembly(InfrastructureAssembly)
			.That()
			.Inherit(typeof(Migration))
			.Should()
			.ResideInNamespace("Infrastructure.Migrations")
			.GetResult();
		result.IsSuccessful.Should().BeTrue();
	}
}