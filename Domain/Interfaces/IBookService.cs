using BookAPI.Domain.Entities;

namespace BookAPI.Domain.Interfaces;
public interface IBookService
{
    List<Book> AllBooks(int? page = 1, string? name = null, string? category = null, string? author = null, string? dateRelease = null);
    Book? SearchForId(int id);
    void Include(Book book);
    void Update(Book book);
    void Delete(Book book);
}