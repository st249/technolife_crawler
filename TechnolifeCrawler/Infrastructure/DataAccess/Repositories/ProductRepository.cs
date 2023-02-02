using Microsoft.EntityFrameworkCore;
using TechnolifeCrawler.Abstractions.DataAccess.Repositories;
using TechnoligeCrawler.Models.BaseModels;

namespace TechnolifeCrawler.Infrastructure.DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly TechnolifeCrawlerDbContext _context;

        public ProductRepository(TechnolifeCrawlerDbContext context)
        {
            _context = context;
        }

        public async Task InserNewProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsProductExistsAsync(int technolifeId)
        {
            return await _context.Products.AnyAsync(e => e.TechnolifeId == technolifeId);
        }
    }
}
