using StoreVisitTracking.Application.DTOs.Product;
using StoreVisitTracking.Domain.Entities;

public interface IProductService
{
    Task<IEnumerable<GetAllProductDto>> GetAllAsync(int page, int pageSize);
    Task CreateAsync(CreateProductDto dto);
}