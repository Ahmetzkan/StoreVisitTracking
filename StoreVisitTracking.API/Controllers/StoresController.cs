using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreVisitTracking.Application.DTOs;
using StoreVisitTracking.Application.DTOs.Store;
using StoreVisitTracking.Application.Interfaces;
using StoreVisitTracking.Application.Paginate;

namespace StoreVisitTracking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StoresController : ControllerBase
{
    private readonly IStoreService _storeService;

    public StoresController(IStoreService storeService)
    {
        _storeService = storeService;
    }

    [HttpGet]
    //[Authorize]
    public async Task<IActionResult> GetAll([FromQuery] PageRequest pageRequest)
    {
        var result = await _storeService.GetAllAsync(pageRequest);
        return Ok(result);
    }

    [HttpPost]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateStoreDto dto)
    {
        await _storeService.CreateAsync(dto);
        return Ok(new { message = "Store created successfully" });
    }

    [HttpPut("{storeId}")]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid storeId, [FromBody] UpdateStoreDto dto)
    {
        await _storeService.UpdateAsync(storeId, dto);
        return Ok(new { message = "Store updated successfully" });
    }

    [HttpDelete("{storeId}")]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid storeId)
    {
        await _storeService.DeleteAsync(storeId);
        return Ok(new { message = "Store deleted successfully" });
    }
}
