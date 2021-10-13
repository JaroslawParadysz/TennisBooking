using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TennisBookings.Merchandise.Api.Data;
using TennisBookings.Merchandise.Api.DomainModels;

namespace TennisBooking.Merchandise.Api.Integration.Tests.Fakes
{
    public class FakeProductDataRepository : IProductDataRepository
    {
        public Task<AddProductResult> AddProductAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyCollection<Product>> GetProductsAsync()
        {
            await Task.CompletedTask;
            return Enumerable.Empty<Product>().ToList();
        }

        public Task<IReadOnlyCollection<Product>> GetProductsForCategoryAsync(string category)
        {
            throw new NotImplementedException();
        }
    }
}
