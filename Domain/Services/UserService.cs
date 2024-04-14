using BookAPI.Domain.DTOs;
using BookAPI.Domain.Entities;
using BookAPI.Domain.Interfaces;
using BookAPI.Infraestruture.DB;

namespace BookAPI.Domain.Services;
public class UserService : IUserService
{
    private readonly DBConnectContext _context;

    public UserService(DBConnectContext context)
    {
        _context = context;
    }

    public User? Login(LoginDTO loginDTO)
    {
        var user = _context.Users.Where(x => x.Email == loginDTO.Email && x.Password == loginDTO.Password).FirstOrDefault();
        return user;
    }

    public User Include(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();

        return user;
    }

    public User? SearchForId(int id)
    {
        return _context.Users.Where(v => v.Id == id).FirstOrDefault();
    }

    public List<User> AllUsers(int? page)
    {
        var query = _context.Users.AsQueryable();

        int itensForPage = 10;

        if (page != null)
        {
            query = query.Skip(((int)page - 1) * itensForPage).Take(itensForPage);
        }

        return [.. query];
    }

    public void Delete(int id)
    {
        var users = _context.Users.FirstOrDefault(x => x.Id == id);

        if (users != null)
            _context.Users.Remove(users);
            _context.SaveChanges();    
    }
}