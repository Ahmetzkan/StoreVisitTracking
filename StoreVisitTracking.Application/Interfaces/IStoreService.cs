using StoreVisitTracking.Application.DTOs;
using StoreVisitTracking.Application.DTOs.Store;
using StoreVisitTracking.Application.DTOs.Visit;
using StoreVisitTracking.Application.Paginate;

public interface IStoreService
{
    Task<IPaginate<GetAllStoreDto>> GetAllAsync(PageRequest pageRequest);
    Task CreateAsync(CreateStoreDto dto);
    Task UpdateAsync(Guid id, UpdateStoreDto dto);
    Task DeleteAsync(Guid id);
}
