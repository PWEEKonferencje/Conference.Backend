using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration;

public class ConferenceConfiguration : IEntityTypeConfiguration<Conference>
{
	public void Configure(EntityTypeBuilder<Conference> builder)
	{
		builder.Property(x => x.Name)
			.HasMaxLength(150)
			.IsRequired()
			.IsUnicode();

		builder.Property(x => x.Description)
			.HasMaxLength(1000);
		
		builder.Property(x => x.StartDate)
			.HasColumnType("timestamp without time zone");
		
		builder.Property(x => x.EndDate)
			.HasColumnType("timestamp without time zone");
		
		builder.Property(x => x.ArticlesDeadline)
			.HasColumnType("timestamp without time zone");
		
		builder.Property(x => x.RegistrationDeadline)
			.HasColumnType("timestamp without time zone");

		builder.OwnsOne(x => x.Address);

		builder.Property(x => x.Address.AddressLine1)
			.HasMaxLength(200);

		builder.Property(x => x.Address.AddressLine2)
			.HasMaxLength(200);

		builder.Property(x => x.Address.PlaceName)
			.HasMaxLength(200);

		builder.Property(x => x.Address.City)
			.HasMaxLength(100);

		builder.Property(x => x.Address.ZipCode)
			.HasMaxLength(15);

		builder.Property(x => x.Address.Country)
			.HasMaxLength(150);
	}
}