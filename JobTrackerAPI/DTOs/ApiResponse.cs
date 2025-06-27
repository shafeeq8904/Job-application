namespace JobTrackerAPI.DTOs;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public object? Errors { get; set; }

    public static ApiResponse<T> SuccessResponse(string message, T data) =>
        new() { Success = true, Message = message, Data = data };

    public static ApiResponse<T> ErrorResponse(string message, object? errors = null) =>
        new() { Success = false, Message = message, Data = default, Errors = errors };
}
