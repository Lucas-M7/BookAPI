using BookAPI.Domain.DTOs;
using BookAPI.Domain.ModelViews;

namespace API.Domain.Validations;
public class BookValidator
{
    public ValidationError ValidateBook(BookDTO bookDTO)
    {
        var validation = new ValidationError
        {
            Messages = []
        };

        if (string.IsNullOrEmpty(bookDTO.Title))
            validation.Messages.Add("Title Empty.");

        if (string.IsNullOrEmpty(bookDTO.Category))
            validation.Messages.Add("Category Empty.");

        if (string.IsNullOrEmpty(bookDTO.UserName))
            validation.Messages.Add("Username Empty.");    

        return validation;
    }
}