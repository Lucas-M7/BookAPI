using System.Text.RegularExpressions;
using BookAPI.Domain.DTOs;
using BookAPI.Domain.ModelViews;
using BookAPI.Infraestruture.DB;

namespace API.Domain.Validations;
public class UserValidator(DBConnectContext dBConnect)
{
    private readonly DBConnectContext _dbConnect = dBConnect;

    public ValidationError Validate(UserDTO userDTO)
    {
        var validation = new ValidationError()
        {
            Messages = []
        };

        string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        string namePattern = @"^[a-zA-Z]+$";

        if (!Regex.IsMatch(userDTO.Email, emailPattern))
            validation.Messages.Add("Invalid email. Make sure you use the right characters.");

        if (_dbConnect.Users.Any(e => e.Email == userDTO.Email))
            validation.Messages.Add("Email already exists.");

        if (!Regex.IsMatch(userDTO.Name, namePattern))
            validation.Messages.Add("Invalid name. Only letters are allowed.");

        if (_dbConnect.Users.Any(n => n.Name == userDTO.Name))
            validation.Messages.Add("Username already exists.");

        if (string.IsNullOrEmpty(userDTO.Password) || userDTO.Password.Length < 4)
            validation.Messages.Add("Check that the password is empty or has at least 4 characters.");

        if (userDTO.Profile == null)
            validation.Messages.Add("Invalid profile.");

        return validation;
    }
}