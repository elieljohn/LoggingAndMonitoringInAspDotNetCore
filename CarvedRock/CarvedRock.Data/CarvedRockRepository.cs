using System.Diagnostics;
using CarvedRock.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CarvedRock.Data
{
    public class CarvedRockRepository :ICarvedRockRepository
    {
        private readonly LocalContext _ctx;
        private readonly ILogger<CarvedRockRepository> _logger;

        public CarvedRockRepository(LocalContext ctx, ILogger<CarvedRockRepository> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }
        public async Task<List<Product>> GetProductsAsync(string category)
        {
            _logger.LogInformation("Getting products in repository for {category}", category);
            return await _ctx.Products.Where(p => p.Category == category || category == "all").ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _ctx.Products.FindAsync(id);
        }

        public List<Product> GetProducts(string category)
        {
            return _ctx.Products.Where(p => p.Category == category || category == "all").ToList();
        }

        public Product? GetProductById(int id)
        {
            var timer = new Stopwatch();
            timer.Start();
            var product = _ctx.Products.Find(id);
            timer.Stop();

            _logger.LogDebug("Querrying products for {id} finished in {milliseconds} milliseconds",
                id, timer.ElapsedMilliseconds);

            return product;
        }
    }
}
