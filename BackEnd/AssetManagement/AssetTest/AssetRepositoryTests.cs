using AssetDAL.DataAccess;
using AssetDAL.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace AssetHubTest
{
    public class AssetRepositoryTests
    {
        private SqliteConnection _connection;
        private AssetManagementDbContext _context;
        private AssetRepository _repository;

        [SetUp]
        public void Setup()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<AssetManagementDbContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new AssetManagementDbContext(options);
            _context.Database.EnsureCreated();

            _repository = new AssetRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
            _connection.Close();
        }

        [Test]
        public async Task CreateAsync_ShouldAddAsset()
        {
            var asset = new Asset
            {
                AssetNo = "A123",
                AssetName = "Dell Laptop",
                AssetCategory = "Electronics",
                AssetModel = "XPS 13",
                ManufacturingDate = DateTime.UtcNow.AddYears(-1),
                ExpiryDate = DateTime.UtcNow.AddYears(2),
                AssetValue = 95000,
                AssetStatus = "Available",
                Description = "Ultrabook",
                CreatedDate = DateTime.UtcNow
            };

            var created = await _repository.CreateAsync(asset);
            var fetched = await _repository.GetByIdAsync(created.AssetId);

            Assert.IsNotNull(fetched);
            Assert.AreEqual("Dell Laptop", fetched?.AssetName);
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveAsset()
        {
            var asset = await _repository.CreateAsync(new Asset
            {
                AssetNo = "A456",
                AssetName = "Monitor",
                AssetCategory = "Electronics",
                AssetStatus = "Available",
                CreatedDate = DateTime.UtcNow
            });

            var deleted = await _repository.DeleteAsync(asset.AssetId);
            var result = await _repository.GetByIdAsync(asset.AssetId);

            Assert.IsTrue(deleted);
            Assert.IsNull(result);
        }

        [Test]
        public async Task UpdateAsync_ShouldModifyAsset()
        {
            var asset = await _repository.CreateAsync(new Asset
            {
                AssetNo = "A789",
                AssetName = "Keyboard",
                AssetCategory = "Peripherals",
                AssetStatus = "Available",
                CreatedDate = DateTime.UtcNow
            });

            asset.AssetName = "Mechanical Keyboard";
            var updated = await _repository.UpdateAsync(asset);

            Assert.AreEqual("Mechanical Keyboard", updated.AssetName);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAssets()
        {
            await _repository.CreateAsync(new Asset
            {
                AssetNo = "A001",
                AssetName = "Mouse",
                AssetCategory = "Peripherals",
                AssetStatus = "Available",
                CreatedDate = DateTime.UtcNow
            });

            var allAssets = await _repository.GetAllAsync();

            Assert.IsNotEmpty(allAssets);
        }
    }
}
