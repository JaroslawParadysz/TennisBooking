using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TennisBookings.Merchandise.Api.Data.Dto;
using TennisBookings.Merchandise.Api.External.Database;

namespace TennisBooking.Merchandise.Api.Integration.Tests.Fakes
{
    public class FakeCloudDatabase : ICloudDatabase
    {
        private List<ProductDto> Products;
        private IReadOnlyCollection<ProductDto> _customDefaultProduct;

        public FakeCloudDatabase(IReadOnlyCollection<ProductDto> products = null)
        {
            ReplaceCustomProducts(products);
            ResetDefaultProducts();
        }

        public Task<ProductDto> GetAsync(string id)
        {
            return Task.FromResult(Products.SingleOrDefault(x => x.Id.ToString() == id));
        }

        public async Task InsertAsync(string id, ProductDto product)
        {
            await Task.CompletedTask;
            Products.Add(product);
        }

        public async Task<IReadOnlyCollection<ProductDto>> ScanAsync()
        {
            await Task.CompletedTask;
            return Products as IReadOnlyCollection<ProductDto>;
        }

        public void ReplaceCustomProducts(IReadOnlyCollection<ProductDto> products)
        {
            _customDefaultProduct = products;
        }

        public void ResetDefaultProducts(bool useCustomIfAvailable = true)
        {
            Products = _customDefaultProduct is object && useCustomIfAvailable ? _customDefaultProduct.ToList()
                : GetDefaultProducts();
        }

        public static FakeCloudDatabase WithDefaultProduct()
        {
            var database = new FakeCloudDatabase();
            database.ResetDefaultProducts();
            return database;
        }

        private List<ProductDto> GetDefaultProducts() => new List<ProductDto>
        {
            new ProductDto
            {
                Id = new Guid(),
                Name = "Test Product 1",
                ShortDescription = "Test Product 1 Description",
                InternalReference = "SKU001",
                Category = "Clothing",
                Price = 30.00m,
                Ratings = new [] { 4, 5, 4, 4, 4 },
                IsEnabled = true,
                StockCount = 1200
            },
            new ProductDto
            {
                Id = new Guid(),
                Name = "Test Product 2",
                ShortDescription = "Test Product 2 Description",
                InternalReference = "SKU002",
                Category = "Clothing",
                Price = 30.00m,
                Ratings = new [] { 4,5,4,4,4 },
                IsEnabled = true,
                StockCount = 1200
            }
        };
    }
}
