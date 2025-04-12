using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using StoreVisitTracking.Application.DTOs.Product;
using StoreVisitTracking.Application.DTOs.Visit;
using StoreVisitTracking.Application.Interfaces;
using StoreVisitTracking.Application.Paginate;
using StoreVisitTracking.Domain.Entities;
using StoreVisitTracking.Infrastructure;

public class ProductService : IProductService
{
    private readonly StoreVisitTrackingDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateProductDto> _validator;
    private readonly ICacheService _cacheService;
    private const string ProductsCachePrefix = "products_";

    public ProductService(
        StoreVisitTrackingDbContext context,
        IMapper mapper,
        IValidator<CreateProductDto> validator,
        ICacheService cacheService)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
        _cacheService = cacheService;
    }

    public async Task<IPaginate<GetAllProductDto>> GetAllAsync(PageRequest pageRequest)
    {
        var cacheKey = $"{ProductsCachePrefix}all_{pageRequest.PageIndex}_{pageRequest.PageSize}";

        if (await _cacheService.ExistsAsync(cacheKey))
        {
            return await _cacheService.GetAsync<IPaginate<GetAllProductDto>>(cacheKey);
        }

        var query = _context.Products.AsQueryable();
        var paginatedResult = await query.ToPaginateAsync(pageRequest.PageIndex, pageRequest.PageSize);

        await _cacheService.SetAsync(cacheKey, paginatedResult.MapPaginate<Product, GetAllProductDto>(_mapper),
            TimeSpan.FromMinutes(15));

        return paginatedResult.MapPaginate<Product, GetAllProductDto>(_mapper);
    }

    public async Task CreateAsync(CreateProductDto dto)
    {
        await _validator.ValidateAndThrowAsync(dto);

        var product = _mapper.Map<Product>(dto);
        product.Id = Guid.NewGuid();
        product.CreatedAt = DateTime.UtcNow;

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        await _cacheService.RemoveByPrefixAsync(ProductsCachePrefix);
    }
}