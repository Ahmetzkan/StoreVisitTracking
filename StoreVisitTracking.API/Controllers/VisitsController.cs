using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreVisitTracking.Application.DTOs.Photo;
using StoreVisitTracking.Application.DTOs.Visit;
using StoreVisitTracking.Application.Paginate;
using StoreVisitTracking.Domain.Entities;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class VisitsController : ControllerBase
{
    private readonly IVisitService _visitService;

    public VisitsController(IVisitService visitService)
    {
        _visitService = visitService;
    }

    [HttpPost]
    [Authorize(Roles = "Standard")]
    public async Task<IActionResult> Create([FromBody] CreateVisitDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        await _visitService.CreateAsync(userId, dto);
        return Ok(new { message = "Visit created successfully" });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] PageRequest pageRequest)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var isAdmin = User.IsInRole("Admin");

        var result = await _visitService.GetAllAsync(userId, isAdmin, pageRequest);
        return Ok(result);
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
    [Authorize(Roles = "Standard")]
    public async Task<IActionResult> UploadPhoto(Guid visitId, [FromBody] PhotoDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        await _visitService.UploadPhotoAsync(userId, visitId, dto);
        return Ok(new { message = "Photo uploaded successfully" });
    }

    [HttpPut("{visitId}/complete")]
    [Authorize(Roles = "Standard")]
    public async Task<IActionResult> CompleteVisit(Guid visitId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        await _visitService.CompleteVisitAsync(userId, visitId);
        return Ok(new { message = "Visit marked as completed" });
    }
}
