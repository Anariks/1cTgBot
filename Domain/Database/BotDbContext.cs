using Contracts.Database;
using Microsoft.EntityFrameworkCore;

namespace Domain.Database;

public class BotDbContext : DbContext
{
    public DbSet<Category> Categories { get; init; }
    public DbSet<Product> Products { get; init; }
    public DbSet<Brand> Brands { get; init; }
    public DbSet<Variation> Variations { get; init; }
    public DbSet<PriceType> PriceTypes { get; init; }
    public DbSet<Storage> Storages { get; init; }
    public DbSet<VariationStock> VariationStocks { get; init; }
    public DbSet<VariationPrice> VariationPrices { get; init; }
    public DbSet<User> Users { get; init; }
    public DbSet<UserRole> UserRoles { get; init; }

    public BotDbContext() : base()
    {
    }
    public BotDbContext(DbContextOptions<BotDbContext> options) : base(options)
    {
    }
    // protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseMySql("{ConntectionString}", new MySqlServerVersion(new Version(8, 0, 26)));
    // }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VariationPrice>()
            .HasKey(k => new { k.VariationId, k.PriceTypeId });

        modelBuilder.Entity<VariationStock>()
            .HasKey(k => new { k.VariationId, k.StorageId });

        modelBuilder.Entity<UserRole>()
            .Property(k => k.PriceTypesId)
            .HasConversion(
                v => string.Join(',', v, null),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());       
        
        modelBuilder.Entity<UserRole>()
            .Property(k => k.StoragesId)
            .HasConversion(
                v => string.Join(',', v, null),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());       
    }
}