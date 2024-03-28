using Microsoft.EntityFrameworkCore;
using Tests.Classes;

namespace Tests.TranslationTests;

/// <summary>
/// DB context.
/// </summary>
public class DataContext : DbContext
{
    public DbSet<Notebook> Notebook { get; set; } = null!;

    /// <inheritdoc />
    public DataContext() : base() {}

    /// <inheritdoc />
    public DataContext(DbContextOptions<DataContext> options) 
        : base(options) {}

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Notebook>(builder =>
        {
            builder.HasKey(s => s.Id);
        });
    }
}