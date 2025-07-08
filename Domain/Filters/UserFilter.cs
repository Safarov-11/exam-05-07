namespace Domain.Filters;

public class UserFilter : ValidFilter
{
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}
