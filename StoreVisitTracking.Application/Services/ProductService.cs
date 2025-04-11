using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using StoreVisitTracking.Application.DTOs.Product;
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

    public async Task<IEnumerable<GetAllProductDto>> GetAllAsync(int page, int pageSize)
    {
        var products = await _context.Products
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return _mapper.Map<IEnumerable<GetAllProductDto>>(products);
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