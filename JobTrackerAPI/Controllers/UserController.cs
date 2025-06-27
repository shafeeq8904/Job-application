using JobTrackerAPI.DTOs;
using JobTrackerAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobTrackerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserCreateDto request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
            );

            return BadRequest(ApiResponse<UserResponseDto>.ErrorResponse("Validation failed", errors));
        }

        var user = await _userService.GetByCredentialsAsync(request.Name, request.Role, request.Password);
        if (user != null)
        {
            var userDto = new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Role = user.Role
            };

            return Ok(ApiResponse<UserResponseDto>.SuccessResponse("Login successful", userDto));
        }

        return NotFound(ApiResponse<UserResponseDto>.ErrorResponse("Login failed: Invalid credentials"));
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateDto request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
            );

            return BadRequest(ApiResponse<UserResponseDto>.ErrorResponse("Validation failed", errors));
        }

        try
        {
            var user = await _userService.CreateUserAsync(request);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id },
                ApiResponse<UserResponseDto>.SuccessResponse("User created", user));
        }
        catch (Exception ex)
        {
            return Conflict(ApiResponse<UserResponseDto>.ErrorResponse("User creation failed", new { general = new[] { ex.Message } }));
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound(ApiResponse<UserResponseDto>.ErrorResponse("User not found"));
        }

        return Ok(ApiResponse<UserResponseDto>.SuccessResponse("User fetched", user));
    }


    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        if (users == null)
        {
            return NotFound(ApiResponse<UserResponseDto>.ErrorResponse("Users not found"));
        }

        return Ok(ApiResponse<List<UserResponseDto>>.SuccessResponse("Users fetched successfully", users));
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
            );

            return BadRequest(ApiResponse<UserResponseDto>.ErrorResponse("Validation failed", errors));
        }

        try
        {
            var updatedUser = await _userService.UpdateUserAsync(id, request);
            if (updatedUser == null)
            {
                return NotFound(ApiResponse<UserResponseDto>.ErrorResponse("User not found"));
            }

            return Ok(ApiResponse<UserResponseDto>.SuccessResponse("User updated", updatedUser));
        }
        catch (Exception ex)
        {
            return Conflict(ApiResponse<UserResponseDto>.ErrorResponse("User update failed", new { general = new[] { ex.Message } }));
        }
    }
}
