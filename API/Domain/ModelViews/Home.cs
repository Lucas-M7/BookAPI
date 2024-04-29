namespace BookAPI.Domain.ModelViews;
public struct Home
{
    public readonly string Message { get => "Welcome the API of Books and Users"; }
    public readonly string Documentation { get => "/swagger"; }
}