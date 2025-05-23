using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public class ConferenceDbContext(DbContextOptions<ConferenceDbContext> options)
	: IdentityDbContext<Identity>(options)
{
	public DbSet<User> UserProfiles { get; set; } = default!;
	public DbSet<Identity> UserIdentities { get; set; } = default!;
	public DbSet<Affiliation> Affiliations { get; set; } = default!;
	public DbSet<Attendee> Attendees { get; set; } = default!;
	public DbSet<Conference> Conferences { get; set; } = default!;
	public DbSet<AttendeeRole> AttendeeRoles { get; set; } = default!;
	public DbSet<UserSnapshot> UserSnapshots { get; set; } = default!;
	public DbSet<Invitation> Invitations { get; set; } = default!;
}