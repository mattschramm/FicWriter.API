using FicWriter.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FicWriter.API.Infrastructure.Data;

public class FicWriterDbContext(DbContextOptions<FicWriterDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
}
