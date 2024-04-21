namespace BookAPI.Domain.ModelViews;
public class BookModelView
{
    public int Id { get; set; }
    public string UserName { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Category { get; set; } = default!;
    public string Author { get; set; } = default!;
    public string DateRelease { get; set; } = default!;
    public bool Readed { get; set; }

}