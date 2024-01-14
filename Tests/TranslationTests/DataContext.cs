using Microsoft.EntityFrameworkCore;
using Tests.Classes.Source;

namespace Tests.TranslationTests;

public class DataContext : DbContext
{
    public DbSet<SourceObject> SourceObjects { get; set; }
    public DbSet<SourceItem> SourceItems { get; set; }

    public DataContext() : base() { }
    public DataContext(DbContextOptions<DataContext> options) : base(options) {}

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