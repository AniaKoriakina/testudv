using System.Linq.Expressions;
using System.Text.Json;
using testudv.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace testudv.Infrastructure.Data;


public class ApplicationDbContext : DbContext
{
    public DbSet<PostInfo>? PostsInfo { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=postgres;Port=5432;Database=vkposts;Username=postgres;Password=7825;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<PostInfo>(entity =>
        {
            entity.ToTable("PostsInfo");
            entity.HasKey(p => p.Id);
            
            entity.Property(p => p.Domain).IsRequired();
            entity.Property(p => p.Count).IsRequired();
            
            var converter = new ValueConverter<Dictionary<char, int>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null), 
                v => JsonSerializer.Deserialize<Dictionary<char, int>>(v, (JsonSerializerOptions)null) 
            );

            entity.Property(p => p.LettersCount)
                .HasConversion(converter)
                .HasColumnType("jsonb");
        });
    }
}