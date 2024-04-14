namespace BookAPI.Domain.ModelViews;
public class UserModelView
{
    public int Id { get; set; }
    public string Email { get; set; } = default!;
    public string Profile { get; set; } = default!;
}