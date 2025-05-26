using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration;

public class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
{
	public void Configure(EntityTypeBuilder<Invitation> builder)
	{
		builder.HasKey(x => x.ConferenceId);
		builder.HasOne(x => x.Conference);
		builder.Property(x => x.Type).HasConversion(x => x.ToString(), x => Enum.Parse<InvitationType>(x));
	}
}