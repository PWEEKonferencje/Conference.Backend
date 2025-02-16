using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration;

public class AttendeeRoleConfiguration : IEntityTypeConfiguration<AttendeeRole> 
{
	public void Configure(EntityTypeBuilder<AttendeeRole> builder)
	{
		builder.HasIndex(x => new { x.Id, Role = x.RoleEnum })
			.IsUnique();
	}
}