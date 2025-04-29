using FicWriter.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FicWriter.API.Infrastructure.Data;

public class FicWriterDbContext(DbContextOptions<FicWriterDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;
    public DbSet<Work> Works { get; set; } = default!;
    public DbSet<Draft> Drafts { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FicWriterDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
