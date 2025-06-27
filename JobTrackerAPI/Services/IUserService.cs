using JobTrackerAPI.Models;
using JobTrackerAPI.DTOs;

namespace JobTrackerAPI.Services;

public interface IUserService
{
    Task<User?> GetByNameAndRoleAsync(string name, string role);
    Task<UserResponseDto> CreateUserAsync(UserCreateDto dto);
    Task<List<UserResponseDto>> GetAllUsersAsync();
    Task<UserResponseDto?> UpdateUserAsync(int id, UserUpdateDto dto);


    Task<User?> GetByCredentialsAsync(string name, string role, string password);

    Task<UserResponseDto?> GetUserByIdAsync(int id);
}
