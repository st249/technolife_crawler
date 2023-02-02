using TechnoligeCrawler.Models.BaseModels;

namespace TechnolifeCrawler.Abstractions.DataAccess.Repositories
{
    public interface IProductRepository
    {
        public Task<bool> IsProductExistsAsync(int technolifeId);
        public Task InserNewProductAsync(Product product);

    }
}
