using BookAPI.Domain.DTOs;
using BookAPI.Domain.Entities;

namespace BookAPI.Domain.Interfaces;
public interface IUserService
{
    User? Login(LoginDTO loginDTO);
    void Delete(int id);
    User Include(User user);
    User? SearchForId(int id);
    List<User> AllUsers(int? page);
}