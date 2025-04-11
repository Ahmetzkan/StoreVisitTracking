using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using StoreVisitTracking.Application.DTOs.Product;
using StoreVisitTracking.Application.DTOs.Visit;
using StoreVisitTracking.Application.Paginate;
using StoreVisitTracking.Domain.Entities;
using StoreVisitTracking.Infrastructure;

public class ProductService : IProductService
{
    private readonly StoreVisitTrackingDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateProductDto> _validator;

    public ProductService(StoreVisitTrackingDbContext context, IMapper mapper, IValidator<CreateProductDto> validator)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
    }
    public async Task<IPaginate<GetAllProductDto>> GetAllAsync(PageRequest pageRequest)
    {
        var query = _context.Products.AsQueryable();
        var paginatedResult = await query.ToPaginateAsync(pageRequest.PageIndex, pageRequest.PageSize);

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
    }
}