using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration;

public class AttendeeConfiguration : IEntityTypeConfiguration<Attendee>
{
	public void Configure(EntityTypeBuilder<Attendee> builder)
	{
		builder.Property(x => x.CreatedAt)
		   .HasColumnType("timestamp without time zone");
	}
}