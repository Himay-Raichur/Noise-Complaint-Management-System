namespace NoiseComplaint.DTOs;

public record RegisterDto(
    string Name,
    string Email,
    string Password,
    string? Role
);