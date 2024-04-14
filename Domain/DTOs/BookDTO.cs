namespace BookAPI.Domain.DTOs;
public class BookDTO
{
    public string Name { get; set; } = default!;
    public string Category { get; set; } = default!;
    public string Author { get; set; } = default!;
    public string DateRelease { get; set; } = default!;
}