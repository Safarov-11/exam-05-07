using Domain.ApiResponse;
using Domain.DTOs;
using Domain.Filters;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin, Manager")]

public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet("by-id")]
    public async Task<Response<UserDTO?>> GetUserByIdAsync(string id)
    {
        return await userService.GetUserAsync(id);
    }

    [HttpGet]
    public async Task<Response<List<UserDTO>>> GetUsersAsync()
    {
        return await userService.GetUsersAsync();
    }

    [HttpGet("byte-filters")]
    public async Task<Response<List<UserDTO>>> GetUsersAsync(UserFilter filter)
    {
        return await userService.GetUsersAsync(filter);
    }

    [HttpPut]
    public async Task<Response<bool>> UpdateUserAsync(UserDTO dTO, string? newRole)
    {
        return await userService.UpdateUserAsync(dTO, newRole);
    }

    [HttpDelete]
    public async Task<Response<bool>> DeleteUserAsync(string id)
    {
        return await userService.DeleteUserAsync(id);
    }
}
