using BookAPI.Domain.Enuns;

namespace BookAPI.Domain.DTOs;
public class UserDTO
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public Profile? Profile { get; set; } = default!;
}