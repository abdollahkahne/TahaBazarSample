using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SampleForTahaBazar.Entities;
using SampleForTahaBazar.Entities.Models;

namespace SampleForTahaBazar.Services
{
    public interface IProductsService
    {
        IEnumerable<Product> GetProducts();
        Task<int> AddProduct(Product product);
    }
    public class ProductsService : IProductsService
    {
        private readonly SaleDbContext _dbContext;

        public ProductsService(SaleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddProduct(Product product)
        {
            _dbContext.Add(product);
            return await _dbContext.SaveChangesAsync();
        }

        public IEnumerable<Product> GetProducts()
        {
            return _dbContext.Products.ToList();
        }
    }

}