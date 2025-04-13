using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Moq;
using StoreVisitTracking.Application.DTOs.Store;
using StoreVisitTracking.Application.Paginate;
using StoreVisitTracking.Domain.Entities;
using StoreVisitTracking.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace StoreVisitTracking.Tests.Services
{
    public class StoreServiceTests : IDisposable
    {
        private readonly StoreVisitTrackingDbContext _dbContext;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IValidator<CreateStoreDto>> _mockCreateValidator;
        private readonly Mock<IValidator<UpdateStoreDto>> _mockUpdateValidator;
        private readonly StoreService _storeService;

        public StoreServiceTests()
        {
            var options = new DbContextOptionsBuilder<StoreVisitTrackingDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            _dbContext = new StoreVisitTrackingDbContext(options);
            _dbContext.Database.EnsureCreated();

            _mockMapper = new Mock<IMapper>();
            _mockCreateValidator = new Mock<IValidator<CreateStoreDto>>();
            _mockUpdateValidator = new Mock<IValidator<UpdateStoreDto>>();

            _storeService = new StoreService(
                _dbContext,
                _mockMapper.Object,
                _mockCreateValidator.Object,
                _mockUpdateValidator.Object);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnPaginatedStores()
        {
            var stores = new List<Store>
            {
                new Store
                {
                    Id = Guid.NewGuid(),
                    Name = "Store 1",
                    Location = "Location 1",
                    CreatedAt = DateTime.UtcNow
                },
                new Store
                {
                    Id = Guid.NewGuid(),
                    Name = "Store 2",
                    Location = "Location 2",
                    CreatedAt = DateTime.UtcNow
                }
            };

            await _dbContext.Stores.AddRangeAsync(stores);
            await _dbContext.SaveChangesAsync();

            _mockMapper.Setup(m => m.Map<GetAllStoreDto>(It.IsAny<Store>()))
                .Returns<Store>(s => new GetAllStoreDto
                {
                    Name = s.Name,
                    Location = s.Location
                });

            _mockMapper.Setup(m => m.Map<IList<GetAllStoreDto>>(It.IsAny<IList<Store>>()))
                .Returns<IList<Store>>(s => s.Select(store => new GetAllStoreDto
                {
                    Name = store.Name,
                    Location = store.Location
                }).ToList());

            var pageRequest = new PageRequest { PageIndex = 0, PageSize = 10 };

            var result = await _storeService.GetAllAsync(pageRequest);

            Assert.Equal(2, result.Items.Count());
            Assert.Equal(2, result.TotalCount);
        }

        [Fact]
        public async Task CreateAsync_ValidStore_ShouldAddStore()
        {
            var createDto = new CreateStoreDto
            {
                Name = "New Store",
                Location = "Location"
            };

            var store = new Store
            {
                Id = Guid.NewGuid(),
                Name = createDto.Name,
                Location = createDto.Location,
                CreatedAt = DateTime.UtcNow
            };

            _mockCreateValidator.Setup(x => x.ValidateAsync(createDto, default))
                .ReturnsAsync(new ValidationResult());

            _mockMapper.Setup(x => x.Map<Store>(createDto))
                .Returns(store);

            await _storeService.CreateAsync(createDto);

            var createdStore = await _dbContext.Stores.FirstOrDefaultAsync();
            Assert.NotNull(createdStore);
            Assert.Equal(createDto.Name, createdStore.Name);
            Assert.Equal(createDto.Location, createdStore.Location);
        }

        [Fact]
        public async Task UpdateAsync_ValidStore_ShouldUpdateStore()
        {
            var storeId = Guid.NewGuid();
            var existingStore = new Store
            {
                Id = storeId,
                Name = "Original Store",
                Location = "Original Location",
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.Stores.AddAsync(existingStore);
            await _dbContext.SaveChangesAsync();

            var updateDto = new UpdateStoreDto
            {
                Name = "Updated Store",
                Location = "Updated Location"
            };

            _mockUpdateValidator.Setup(x => x.ValidateAsync(updateDto, default))
                .ReturnsAsync(new ValidationResult());

            _mockMapper.Setup(m => m.Map(updateDto, existingStore))
                .Callback<UpdateStoreDto, Store>((dto, entity) =>
                {
                    entity.Name = dto.Name;
                    entity.Location = dto.Location;
                })
                .Returns(existingStore);

            await _storeService.UpdateAsync(storeId, updateDto);

            var updatedStore = await _dbContext.Stores.FindAsync(storeId);
            Assert.NotNull(updatedStore);
            Assert.Equal(updateDto.Name, updatedStore.Name);
            Assert.Equal(updateDto.Location, updatedStore.Location);
        }

        [Fact]
        public async Task DeleteAsync_ExistingStore_ShouldRemoveStore()
        {
            var storeId = Guid.NewGuid();
            var store = new Store
            {
                Id = storeId,
                Name = "Store to delete",
                Location = "Delete Location",
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.Stores.AddAsync(store);
            await _dbContext.SaveChangesAsync();

            await _storeService.DeleteAsync(storeId);

            var deletedStore = await _dbContext.Stores.FindAsync(storeId);
            Assert.Null(deletedStore);
        }
    }
}