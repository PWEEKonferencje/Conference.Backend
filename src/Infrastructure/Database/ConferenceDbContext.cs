using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public class ConferenceDbContext(DbContextOptions<ConferenceDbContext> options)
	: IdentityDbContext<UserAccount>(options)
{
	public DbSet<UserProfile> UserProfiles { get; set; } = default!;
	public DbSet<UserAccount> UserAccounts { get; set; } = default!;
}