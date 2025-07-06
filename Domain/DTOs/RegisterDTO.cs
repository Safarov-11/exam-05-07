namespace Domain.DTOs;

public class RegisterDTO
{
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Password { get; set; } = null!;
}
