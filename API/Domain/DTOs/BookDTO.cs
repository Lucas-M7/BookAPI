namespace BookAPI.Domain.DTOs;
public class BookDTO
{
    public string UserName { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Category { get; set; } = default!;
    public string Author { get; set; } = default!;
    public string DateRelease { get; set; } = default!;
    public bool Readed { get; set; } = default!;
}