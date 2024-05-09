using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Domain.Validations;
using BookAPI.Domain.DTOs;
using BookAPI.Domain.Entities;
using BookAPI.Domain.Enuns;
using BookAPI.Domain.Interfaces;
using BookAPI.Domain.ModelViews;
using BookAPI.Domain.Services;
using BookAPI.Infraestruture.DB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace BookAPI;

public class Startup
{
    private string Key { get; set; } = default!;
    public IConfiguration Configuration { get; set; } = default!;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        Key = Configuration.GetSection("Jwt").ToString();
    }

    // Configuração dos serviços
    public void ConfigureServices(IServiceCollection services)
    {
        #region JWT
        // Configurando Token JWT
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(option =>
        {
            option.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key)),

                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
        #endregion

        services.AddAuthorization();

        #region Cors
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder => 
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            });
        });
        #endregion

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IBookService, BookService>();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your TokenJWT here."
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "BookAPI",
                Description = "An API where users can register their favorite books"
            });
        });

        services.AddDbContext<DBConnectContext>(options =>
        {
            options.UseSqlServer(Configuration.GetConnectionString("ConexaoPadrao"));
        });
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseCors("AllowSpecifiOrigin");

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            #region Home
            endpoints.MapGet("/wellcome/", () => Results.Json(new Home())).AllowAnonymous().WithTags("Home");
            #endregion

            #region  Users
            // Função para gerar Token na hora do usuário solicitar o login.
            string GenerateTokenJwt(User user)
            {
                if (string.IsNullOrEmpty(Key)) return string.Empty;

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>()
                {
                    new("Email", user.Email),
                    new("Profile", user.Profile),
                    new(ClaimTypes.Role, user.Profile)
                };

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            endpoints.MapPost("/users/login", ([FromBody] LoginDTO loginDTO, IUserService administratorService) =>
            {
                var usr = administratorService.Login(loginDTO);

                if (usr != null)
                {
                    // Chamada da função de gerar o token após a solicitação de login
                    string token = GenerateTokenJwt(usr);

                    return Results.Ok(new UserLogedIn
                    {
                        Email = usr.Email,
                        Name = usr.Name,
                        Profile = usr.Profile,
                        Token = token
                    });
                }
                else
                    return Results.Unauthorized();
            }).AllowAnonymous()
            .WithTags("Users");

            endpoints.MapPost("/users/", ([FromBody] UserDTO userDTO, IUserService userService, DBConnectContext dBConnect) =>
            {
                var userValidator = new UserValidator(dBConnect);
                var validation = userValidator.UserValidate(userDTO);

                if (validation.Messages.Count > 0)
                    return Results.BadRequest(validation);

                var user = new User()
                {
                    Email = userDTO.Email,
                    Name = userDTO.Name,
                    Password = userDTO.Password,
                    Profile = userDTO.Profile?.ToString() ?? Profile.Common.ToString()
                };

                userService.Include(user);

                return Results.Created($"/user/{user.Id}", new UserModelView
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Profile = user.Profile
                });
            })
            .WithTags("Users");

            endpoints.MapGet("/users/", ([FromQuery] int? page, IUserService userService) =>
            {
                var usr = new List<UserModelView>();
                var users = userService.AllUsers(page);
                foreach (var user in users)
                {
                    usr.Add(new UserModelView
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Name = user.Name,
                        Profile = user.Profile
                    });
                }

                return Results.Ok(usr);
            })
            .RequireAuthorization()
            .RequireAuthorization(new AuthorizeAttribute { Roles = "ADM" })
            .WithTags("Users");

            endpoints.MapGet("/users/{id}", ([FromRoute] int id, IUserService userService) =>
            {
                var user = userService.SearchForId(id);

                if (user == null)
                    return Results.NotFound();

                return Results.Ok(new UserModelView
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Profile = user.Profile
                });
            })
            .RequireAuthorization()
            .RequireAuthorization(new AuthorizeAttribute { Roles = "ADM" })
            .WithTags("Users");

            endpoints.MapDelete("/users/{id}", ([FromServices] IUserService userService, int id) =>
            {
                var user = userService.SearchForId(id);

                if (user == null)
                    return Results.NotFound();

                userService.Delete(id);
                return Results.NoContent();
            })
            .RequireAuthorization()
            .RequireAuthorization(new AuthorizeAttribute { Roles = "ADM" })
            .WithTags("Users");
            #endregion

            #region Books

            endpoints.MapPost("/books/", ([FromBody] BookDTO bookDTO, IBookService bookService, DBConnectContext connectContext) =>
            {
                var validation = new BookValidator();
                var bookValid = validation.ValidateBook(bookDTO);

                if (bookValid.Messages.Count > 0)
                    return Results.BadRequest(validation);

                var book = new Book
                {
                    UserName = bookDTO.UserName,
                    Title = bookDTO.Title,
                    Category = bookDTO.Category,
                    Author = bookDTO.Author,
                    DateRelease = bookDTO.DateRelease
                };

                bookService.Include(book);
                return Results.Created($"/book/{book.Id}", book);
            })
            .RequireAuthorization()
            .RequireAuthorization(new AuthorizeAttribute { Roles = "ADM, Common" })
            .WithTags("Books");

            endpoints.MapGet("/books/", ([FromQuery] int? page, IBookService bookService) =>
            {
                var books = bookService.AllBooks(page);

                return Results.Ok(books);
            })
            .RequireAuthorization()
            .RequireAuthorization(new AuthorizeAttribute { Roles = "ADM, Common" })
            .WithTags("Books");

            endpoints.MapGet("/books/{id}", ([FromRoute] int id, IBookService bookService) =>
            {
                var book = bookService.SearchForId(id);

                if (book == null)
                    return Results.NotFound();

                return Results.Ok(new BookModelView
                {
                    Id = book.Id,
                    UserName = book.UserName,
                    Title = book.Title,
                    Category = book.Category,
                    Author = book.Author,
                    DateRelease = book.DateRelease,
                    Readed = book.Readed
                });
            })
            .RequireAuthorization()
            .RequireAuthorization(new AuthorizeAttribute { Roles = "ADM, Common" })
            .WithTags("Books");

            endpoints.MapPut("/books/{id}", ([FromRoute] int id, BookDTO bookDTO, IBookService bookService) =>
            {
                var book = bookService.SearchForId(id);

                if (book == null)
                    return Results.NotFound();

                var validation = new BookValidator();
                var bookValid = validation.ValidateBook(bookDTO);

                if (bookValid.Messages.Count > 0)
                    return Results.BadRequest();

                book.Title = bookDTO.Title;
                book.Category = bookDTO.Category;
                book.Author = bookDTO.Author;
                book.DateRelease = bookDTO.DateRelease;
                book.Readed = bookDTO.Readed;

                bookService.Update(book);

                return Results.Ok();
            })
            .RequireAuthorization()
            .RequireAuthorization(new AuthorizeAttribute { Roles = "ADM" })
            .WithTags("Books");

            endpoints.MapDelete("/books/{id}", ([FromRoute] int id, IBookService bookService) =>
            {
                var book = bookService.SearchForId(id);

                if (book == null)
                    return Results.NotFound();

                bookService.Delete(book);

                return Results.NoContent();
            })
            .RequireAuthorization()
            .RequireAuthorization(new AuthorizeAttribute { Roles = "ADM" })
            .WithTags("Books");
            #endregion
        });
    }
}