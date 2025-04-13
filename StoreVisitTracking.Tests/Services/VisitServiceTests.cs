using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Moq;
using StoreVisitTracking.Domain.Entities;
using StoreVisitTracking.Domain.Enums;
using StoreVisitTracking.Application.DTOs.Photo;
using StoreVisitTracking.Application.DTOs.Visit;
using StoreVisitTracking.Application.Paginate;
using StoreVisitTracking.Application.Services;
using StoreVisitTracking.Infrastructure;
using Xunit;

namespace StoreVisitTracking.Tests.Services
{
    public class VisitServiceTests
    {
        private readonly StoreVisitTrackingDbContext _dbContext;
        private readonly IVisitService _visitService;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IValidator<CreateVisitDto>> _visitValidatorMock;
        private readonly Mock<IValidator<PhotoDto>> _photoValidatorMock;

        public VisitServiceTests()
        {
            var options = new DbContextOptionsBuilder<StoreVisitTrackingDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
                .Options;

            _dbContext = new StoreVisitTrackingDbContext(options);
            
            _mapperMock = new Mock<IMapper>();
            _visitValidatorMock = new Mock<IValidator<CreateVisitDto>>();
            _photoValidatorMock = new Mock<IValidator<PhotoDto>>();

            _mapperMock.Setup(m => m.Map<GetVisitDto>(It.IsAny<Visit>()))
                .Returns((Visit v) => new GetVisitDto 
                { 
                    Id = v.Id,
                    StoreName = v.Store?.Name ?? string.Empty,
                    VisitDate = v.VisitDate,
                    Status = v.Status.ToString(),
                    Photos = new List<PhotoDto>()
                });

            _mapperMock.Setup(m => m.Map<IList<GetVisitDto>>(It.IsAny<IList<Visit>>()))
                .Returns((IList<Visit> v) => v.Select(visit => new GetVisitDto
                {
                    Id = visit.Id,
                    StoreName = visit.Store?.Name ?? string.Empty,
                    VisitDate = visit.VisitDate,
                    Status = visit.Status.ToString(),
                    Photos = new List<PhotoDto>()
                }).ToList());

            _visitService = new VisitService(
                _dbContext,
                _mapperMock.Object,
                _visitValidatorMock.Object,
                _photoValidatorMock.Object
            );
        }

        [Fact]
        public async Task GetAllAsync_AdminUser_ShouldReturnAllVisits()
        {
            var userId = Guid.NewGuid();

            var store = new Store
            {
                Id = Guid.NewGuid(),
                Name = "Test Store",
                Location = "Test Location",
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.Stores.AddAsync(store);

            var visits = new List<Visit>
            {
                new Visit
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Store = store,
                    VisitDate = DateTime.UtcNow,
                    Status = VisitStatus.Completed
                },
                new Visit
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Store = store,
                    VisitDate = DateTime.UtcNow,
                    Status = VisitStatus.Completed
                }
            };

            await _dbContext.Visits.AddRangeAsync(visits);
            await _dbContext.SaveChangesAsync();

            var pageRequest = new PageRequest { PageIndex = 0, PageSize = 10 };

            var result = await _visitService.GetAllAsync(userId, true, pageRequest);

            Assert.Equal(2, result.Items.Count());
            Assert.Equal(2, result.TotalCount);
        }

        [Fact]
        public async Task GetByIdAsync_ValidVisit_ShouldReturnVisit()
        {
            var userId = Guid.NewGuid();
            var visitId = Guid.NewGuid();

            var store = new Store
            {
                Id = Guid.NewGuid(),
                Name = "Test Store",
                Location = "Test Location",
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.Stores.AddAsync(store);

            var visit = new Visit
            {
                Id = visitId,
                UserId = userId,
                Store = store,
                Photos = new List<Photo>(),
                VisitDate = DateTime.UtcNow,
                Status = VisitStatus.Completed
            };

            await _dbContext.Visits.AddAsync(visit);
            await _dbContext.SaveChangesAsync();

            var result = await _visitService.GetByIdAsync(userId, visitId, false);

            Assert.NotNull(result);
            Assert.Equal(visitId, result.Id);
        }
    }
}