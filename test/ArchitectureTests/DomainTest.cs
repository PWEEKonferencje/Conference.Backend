using Domain.Repositories;
using FluentAssertions;
using NetArchTest.Rules;

namespace ArchitectureTests;

public class DomainTest : BaseArchitectureTest
{
	[Fact]
	public void RepositoryInterfaces_Should_OnlyBeInDomain()
	{
		var result = Types.InAssembly(DomainAssembly)
			.That()
			.AreInterfaces()
			.And()
			.Inherit(typeof(IRepository<>))
			.Should()
			.ResideInNamespace("Domain.Repositories")
			.GetResult();
		result.IsSuccessful.Should().BeTrue();
	}
}