using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using StoreVisitTracking.Application.DTOs;
using StoreVisitTracking.Application.DTOs.Store;
using StoreVisitTracking.Application.Interfaces;
using StoreVisitTracking.Domain.Entities;
using StoreVisitTracking.Infrastructure;

public class StoreService : IStoreService
{
    private readonly StoreVisitTrackingDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateStoreDto> _createValidator;
    private readonly IValidator<UpdateStoreDto> _updateValidator;

    public StoreService(StoreVisitTrackingDbContext context, IMapper mapper, IValidator<CreateStoreDto> createValidator, IValidator<UpdateStoreDto> updateValidator)
    {
        _context = context;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<IEnumerable<GetAllStoreDto>> GetAllAsync(int page, int pageSize)
    {
        var stores = await _context.Stores
            .OrderByDescending(s => s.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return _mapper.Map<IEnumerable<GetAllStoreDto>>(stores);
    }

    public async Task CreateAsync(CreateStoreDto dto)
    {
        await _createValidator.ValidateAndThrowAsync(dto);

        var store = _mapper.Map<Store>(dto);
        store.Id = Guid.NewGuid();
        store.CreatedAt = DateTime.UtcNow;

        _context.Stores.Add(store);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, UpdateStoreDto dto)
    {
        await _updateValidator.ValidateAndThrowAsync(dto);

        var store = await _context.Stores.FindAsync(id);
        if (store == null) throw new Exception("Store not found");

        _mapper.Map(dto, store);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var store = await _context.Stores.FindAsync(id);
        if (store == null) throw new Exception("Store not found");

        _context.Stores.Remove(store);
        await _context.SaveChangesAsync();
    }
}