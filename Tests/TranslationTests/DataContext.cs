using Microsoft.EntityFrameworkCore;
using Tests.Classes.Source;

namespace Tests.TranslationTests;

/// <summary>
/// Контекст БД.
/// </summary>
public class DataContext : DbContext
{
    public DbSet<SourceObject> SourceObjects { get; set; } = null!;
    public DbSet<SourceItem> SourceItems { get; set; } = null!;

    /// <inheritdoc />
    public DataContext() 
        : base() {}

    /// <inheritdoc />
    public DataContext(DbContextOptions<DataContext> options) 
        : base(options) {}

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SourceItem>(builder =>
        {
            builder.HasKey(s => s.Id);

            builder.HasOne<SourceObject>()
                .WithMany(s => s.SourceItems)
                .HasForeignKey("source_id");
        });
        
        modelBuilder.Entity<SourceObject>(builder =>
        {
            builder.HasKey(s => s.Id);
        });
    }
}