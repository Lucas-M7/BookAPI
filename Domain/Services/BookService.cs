using BookAPI.Domain.Entities;
using BookAPI.Domain.Interfaces;
using BookAPI.Infraestruture.DB;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Domain.Services;
public class BookService(DBConnectContext context) : IBookService
{
    private readonly DBConnectContext _context = context;

    #region Paginação
    public List<Book> AllBooks(int? page = 1, string? title = null, string? category = null, string? author = null, string? dateRelease = null)
    {
        var query = _context.Books.AsQueryable(); // Converter coleção de elemento em uma consulta LinQ
        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(v => EF.Functions.Like(v.Title.ToLower(), $"%{title.ToLower()}%"));
        }

        // Lógica da paginação dos dados
        int itensForPage = 10;

        if (page != null)
        {
            query = query.Skip((int)(page - 1) * itensForPage).Take(itensForPage);
        }
        return [.. query];
    }
    #endregion

    public void Delete(Book book)
    {
        _context.Books.Remove(book);
        _context.SaveChanges();
    }

    public void Include(Book book)
    {
        _context.Books.Add(book);
        _context.SaveChanges();
    }

    public Book? SearchForId(int id)
    {
        return _context.Books.Where(x => x.Id == id).FirstOrDefault();
    }

    public void Update(Book book)
    {
        _context.Books.Update(book);
        _context.SaveChanges();
    }
}