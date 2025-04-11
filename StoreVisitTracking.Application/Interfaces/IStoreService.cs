using StoreVisitTracking.Application.DTOs;
using StoreVisitTracking.Application.DTOs.Store;

public interface IStoreService
{
    Task<IEnumerable<GetAllStoreDto>> GetAllAsync(int page, int pageSize);
    Task CreateAsync(CreateStoreDto dto);
    Task UpdateAsync(Guid id, UpdateStoreDto dto);
    Task DeleteAsync(Guid id);
}
