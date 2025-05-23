using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreVisitTracking.Application.DTOs.Auth;
using StoreVisitTracking.Application.Interfaces;
using StoreVisitTracking.Application.Services;
using StoreVisitTracking.Domain.Enums;

namespace StoreVisitTracking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    
    //[Authorize(Roles = "Admin")]
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
    {
        var result = await _authService.RegisterAsync(registerDto);
        return Ok(result);
    }
    

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
    {

        var result = await _authService.LoginAsync(loginDto);
        return Ok(result);
    }
}