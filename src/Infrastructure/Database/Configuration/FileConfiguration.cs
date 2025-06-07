using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using File = Domain.Entities.File;

namespace Infrastructure.Database.Configuration;

public class FileConfiguration : IEntityTypeConfiguration<File>
{
	public void Configure(EntityTypeBuilder<File> builder)
	{
		builder.HasKey(x => x.Id);
		builder.Property(x => x.Name).HasMaxLength(200);
		builder.Property(x => x.Extension).HasMaxLength(10);
	}
}