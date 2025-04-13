using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using StoreVisitTracking.Application.DTOs;
using StoreVisitTracking.Application.DTOs.Store;
using StoreVisitTracking.Application.DTOs.Visit;
using StoreVisitTracking.Application.Interfaces;
using StoreVisitTracking.Application.Messages;
using StoreVisitTracking.Application.Paginate;
using StoreVisitTracking.Domain.Entities;
using StoreVisitTracking.Infrastructure;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

    public async Task<IPaginate<GetAllStoreDto>> GetAllAsync(PageRequest pageRequest)
    {
        var query = _context.Stores.AsQueryable();
        var result = await query.ToPaginateAsync(pageRequest.PageIndex, pageRequest.PageSize);

        return result.MapPaginate<Store, GetAllStoreDto>(_mapper);
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
        if (store == null) throw new Exception(ApplicationMessages.StoreNotFound);

        _mapper.Map(dto, store);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var store = await _context.Stores.FindAsync(id);
        if (store == null) throw new Exception(ApplicationMessages.StoreNotFound);

        _context.Stores.Remove(store);
        await _context.SaveChangesAsync();
    }
}