using StoreVisitTracking.Application.DTOs.Product;
using StoreVisitTracking.Application.DTOs.Visit;
using StoreVisitTracking.Application.Paginate;
using StoreVisitTracking.Domain.Entities;

public interface IProductService
{
    Task<IPaginate<GetAllProductDto>> GetAllAsync(PageRequest pageRequest);

    Task CreateAsync(CreateProductDto dto);
}