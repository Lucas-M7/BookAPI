namespace BookAPI.Domain.ModelViews;
public class UserLogedIn
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Profile { get; set; } = default!;
    public string Token { get; set; } = default!;
}