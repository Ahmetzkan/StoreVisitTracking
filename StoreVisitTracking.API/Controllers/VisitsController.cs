using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreVisitTracking.API.Messages;
using StoreVisitTracking.Application.DTOs.Photo;
using StoreVisitTracking.Application.DTOs.Visit;
using StoreVisitTracking.Application.Interfaces;
using StoreVisitTracking.Application.Paginate;
using StoreVisitTracking.Domain.Entities;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class VisitsController : ControllerBase
{
    private readonly IVisitService _visitService;
    private readonly ICacheService _cacheService;

    public VisitsController(IVisitService visitService, ICacheService cacheService)
    {
        _visitService = visitService;
        _cacheService = cacheService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] PageRequest pageRequest)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var isAdmin = User.IsInRole("Admin");

        var cacheKey = $"visits_{userId}_{isAdmin}_{pageRequest.PageIndex}_{pageRequest.PageSize}";

        if (await _cacheService.ExistsAsync(cacheKey))
        {
            return Ok(await _cacheService.GetAsync<IPaginate<GetVisitDto>>(cacheKey));
        }

        var result = await _visitService.GetAllAsync(userId, isAdmin, pageRequest);

        if (!isAdmin) 
        {
            await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5));
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateVisitDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        await _visitService.CreateAsync(userId, dto);

        await _cacheService.RemoveByPrefixAsync($"visits_{userId}_");

        return Ok(APIMessages.VisitCreatedSuccesfully);
    }


    [HttpGet("{visitId}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid visitId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var isAdmin = User.IsInRole("Admin");
        var result = await _visitService.GetByIdAsync(userId, visitId, isAdmin);
        return Ok(result);
    }

    [HttpPost("{visitId}/photos")]
    [Authorize]
    public async Task<IActionResult> UploadPhoto(Guid visitId, [FromBody] PhotoDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        await _visitService.UploadPhotoAsync(userId, visitId, dto);
        return Ok(APIMessages.PhotoUploadedSuccesfully);
    }

    [HttpPut("{visitId}/complete")]
    [Authorize]
    public async Task<IActionResult> CompleteVisit(Guid visitId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        await _visitService.CompleteVisitAsync(userId, visitId);
        return Ok();
    }
}
