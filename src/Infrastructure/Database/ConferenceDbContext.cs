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
}