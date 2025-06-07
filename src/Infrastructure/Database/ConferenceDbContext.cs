using System.Reflection;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using File = Domain.Entities.File;

namespace Infrastructure.Database;

public class ConferenceDbContext(DbContextOptions<ConferenceDbContext> options)
	: IdentityDbContext<Identity>(options)
{
	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}

	public DbSet<User> UserProfiles { get; set; }
	public DbSet<Identity> UserIdentities { get; set; }
	public DbSet<Affiliation> Affiliations { get; set; }
	public DbSet<Attendee> Attendees { get; set; }
	public DbSet<Conference> Conferences { get; set; }
	public DbSet<Track> Tracks { get; set; }
	public DbSet<AttendeeRole> AttendeeRoles { get; set; }
	public DbSet<UserSnapshot> UserSnapshots { get; set; }
	public DbSet<Invitation> Invitations { get; set; }
	public DbSet<File> Files { get; set; }
	public DbSet<Keyword> Keywords { get; set; }
	public DbSet<Paper> Papers { get; set; }
	public DbSet<PaperFileRevision> PaperFileRevisions { get; set; }
	public DbSet<PaperStatusRevision> PaperStatusRevisions { get; set; }
	public DbSet<Review> Reviews { get; set; }
	public DbSet<ReviewRevision> ReviewRevisions { get; set; }
	public DbSet<ReviewStatusRevision> ReviewStatusRevisions { get; set; }
}