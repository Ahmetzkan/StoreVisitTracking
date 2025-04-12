// StoresController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreVisitTracking.Application.DTOs.Store;
using StoreVisitTracking.Application.Interfaces;
using StoreVisitTracking.Application.Paginate;

[ApiController]
[Route("api/[controller]")]
public class StoresController : ControllerBase
{
    private readonly IStoreService _storeService;
    private readonly ICacheService _cacheService;
    private const string StoresCachePrefix = "stores_";

    public StoresController(IStoreService storeService, ICacheService cacheService)
    {
        _storeService = storeService;
        _cacheService = cacheService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PageRequest pageRequest)
    {
        var cacheKey = $"{StoresCachePrefix}all_{pageRequest.PageIndex}_{pageRequest.PageSize}";

        if (await _cacheService.ExistsAsync(cacheKey))
        {
            var cachedResult = await _cacheService.GetAsync<IPaginate<GetAllStoreDto>>(cacheKey);
            return Ok(cachedResult);
        }

        var result = await _storeService.GetAllAsync(pageRequest);
        await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(10));

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateStoreDto dto)
    {
        await _storeService.CreateAsync(dto);
        await _cacheService.RemoveByPrefixAsync(StoresCachePrefix);
        return Ok(new { message = "Store created successfully" });
    }

    [HttpPut("{storeId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid storeId, [FromBody] UpdateStoreDto dto)
    {
        await _storeService.UpdateAsync(storeId, dto);
        await _cacheService.RemoveByPrefixAsync(StoresCachePrefix);
        return Ok(new { message = "Store updated successfully" });
    }

    [HttpDelete("{storeId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid storeId)
    {
        await _storeService.DeleteAsync(storeId);
        await _cacheService.RemoveByPrefixAsync(StoresCachePrefix);
        return Ok(new { message = "Store deleted successfully" });
    }
}