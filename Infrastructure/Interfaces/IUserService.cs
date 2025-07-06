using Domain.ApiResponse;
using Domain.DTOs;
using Domain.Filters;

namespace Infrastructure.Interfaces;

public interface IUserService
{
    public Task<Response<bool>> UpdateUserAsync(UserDTO userDTO, string? newRole);
    public Task<Response<bool>> DeleteUserAsync(string id);
    public Task<Response<UserDTO?>> GetUserAsync(string id);
    public Task<Response<List<UserDTO>>> GetUsersAsync();
    public Task<Response<List<UserDTO>>> GetUsersAsync(UserFilter filter);
}
