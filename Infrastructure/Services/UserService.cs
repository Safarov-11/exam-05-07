using System.Net;
using Domain.ApiResponse;
using Domain.DTOs;
using Domain.Filters;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class UserService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : IUserService
{
    public async Task<Response<bool>> DeleteUserAsync(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user == null)
        {
            return new Response<bool>("User not found", HttpStatusCode.NotFound);
        }

        var deleted = await userManager.DeleteAsync(user);
        return deleted.Succeeded
        ? new Response<bool>(true)
        : new Response<bool>("Something went wrong", HttpStatusCode.BadRequest);
    }

    public async Task<Response<bool>> UpdateUserAsync(UserDTO userDTO, string? newRole)
    {
        var user = await userManager.FindByIdAsync(userDTO.Id);
        if (user == null)
        {
            return new Response<bool>("User not found", HttpStatusCode.NotFound);
        }

        if (!string.IsNullOrEmpty(newRole))
        {
            if (!await roleManager.RoleExistsAsync(newRole))
            {
                return new Response<bool>("Role does not exist", HttpStatusCode.BadRequest);
            }

            var curentRole = await userManager.GetRolesAsync(user);
            if (curentRole.Any())
            {
                await userManager.RemoveFromRolesAsync(user, curentRole);
            }

            var setNewRole = await userManager.AddToRoleAsync(user, newRole);
            if (!setNewRole.Succeeded)
            {
                return new Response<bool>("Something went wrong", HttpStatusCode.BadRequest);
            }
        }

        user.UserName = userDTO.UserName;
        user.Email = userDTO.Email;
        user.PhoneNumber = userDTO.PhoneNumber;

        var updated = await userManager.UpdateAsync(user);
        return updated.Succeeded
        ? new Response<bool>(true)
        : new Response<bool>("Something went wrong", HttpStatusCode.BadRequest);
    }

    public async Task<Response<UserDTO?>> GetUserAsync(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user == null)
        {
            return new Response<UserDTO?>("User not found", HttpStatusCode.NotFound);
        }

        var userDTO = new UserDTO()
        {
            Id = user.Id,
            UserName = user.UserName!,
            Email = user.Email!,
            PhoneNumber = user.PhoneNumber!
        };

        return new Response<UserDTO?>(userDTO);
    }

    public async Task<Response<List<UserDTO>>> GetUsersAsync()
    {
        var users = await userManager.Users
        .Select(us => new UserDTO
        {
            Id = us.Id,
            UserName = us.UserName!,
            Email = us.Email!,
            PhoneNumber = us.PhoneNumber!
        }).ToListAsync();

        return new Response<List<UserDTO>>(users);
    }

    public async Task<Response<List<UserDTO>>> GetUsersAsync(UserFilter filter)
    {
        var query = userManager.Users;
        if (!string.IsNullOrWhiteSpace(filter.Email))
        {
            query = query.Where(us => us.Email!.Contains(filter.Email));
        }

        if (!string.IsNullOrWhiteSpace(filter.PhoneNumber))
        {
            query = query.Where(us => us.PhoneNumber!.Contains(filter.PhoneNumber));
        }

        var users = await query.Select(us => new UserDTO
        {
            Id = us.Id,
            UserName = us.UserName!,
            Email = us.Email!,
            PhoneNumber = us.PhoneNumber!
        }).ToListAsync();

        return new Response<List<UserDTO>>(users);
    }

}
