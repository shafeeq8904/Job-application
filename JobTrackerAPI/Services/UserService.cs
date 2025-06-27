using JobTrackerAPI.Models;
using JobTrackerAPI.Repositories;
using JobTrackerAPI.DTOs;

namespace JobTrackerAPI.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;

    public UserService(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> GetByNameAndRoleAsync(string name, string role)
    {
        var users = await _userRepository.FindAsync(u => u.Name == name && u.Role == role);
        if (!users.Any())
            throw new Exception("User not found with the provided name and role.");
        return users.FirstOrDefault();
    }

    public async Task<UserResponseDto> CreateUserAsync(UserCreateDto dto)
    {
        var exists = await _userRepository.FindAsync(u => u.Name == dto.Name && u.Role == dto.Role);
        if (exists.Any())
            throw new Exception("User with same name and role already exists.");

        var user = new User
        {
            Name = dto.Name,
            Role = dto.Role,
            Password = dto.Password
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveAsync();

        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Role = user.Role
        };
    }

    public async Task<UserResponseDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return null;

        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Role = user.Role
        };
    }

    public async Task<List<UserResponseDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(u => new UserResponseDto
        {
            Id = u.Id,
            Name = u.Name,
            Role = u.Role
        }).ToList();
    }

    public async Task<User?> GetByCredentialsAsync(string name, string role, string password)
    {
        var users = await _userRepository.FindAsync(
            u => u.Name == name && u.Role == role && u.Password == password
        );

        return users.FirstOrDefault();
    }
    
    public async Task<UserResponseDto?> UpdateUserAsync(int id, UserUpdateDto dto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return null;

        // Optional: Prevent duplicate name + role combination
        var conflict = await _userRepository.FindAsync(u =>
            u.Id != id && u.Name == dto.Name && u.Role == dto.Role);
        if (conflict.Any())
            throw new Exception("Another user with the same name and role already exists.");

        // Update fields
        user.Name = dto.Name;
        user.Role = dto.Role;
        user.Password = dto.Password;

        _userRepository.Update(user);
        await _userRepository.SaveAsync();

        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Role = user.Role
        };
    }

}
