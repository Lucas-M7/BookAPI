namespace BookAPI.Domain.ModelViews;
public struct Home
{
    public readonly string Message { get => "Welcome the API of Book and Users"; }
    public readonly string Documentation { get => "/swagger"; }
}