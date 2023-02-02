using Microsoft.EntityFrameworkCore;
using TechnoligeCrawler.Models.BaseModels;

namespace TechnolifeCrawler.Infrastructure.DataAccess
{
    public class TechnolifeCrawlerDbContext : DbContext
    {
        public TechnolifeCrawlerDbContext(DbContextOptions<TechnolifeCrawlerDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                 .HasIndex(e => e.TechnolifeId)
                 .IsUnique(true);
        }
    }
}
