using Core.Application.Dtos;

namespace Application.Features.Auth.Dtos;

public class UserForRegisterDto : IDto
{
    public string UserName { get; set; }
    public string Email { get; set; }

    public string Password { get; set; }

    public UserForRegisterDto()
    {
        UserName = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
    }

    public UserForRegisterDto(string userName, string email, string password)
    {
        UserName = userName;
        Email = email;
        Password = password;
    }
}
