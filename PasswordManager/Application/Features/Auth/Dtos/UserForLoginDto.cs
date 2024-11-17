using Core.Application.Dtos;

namespace Application.Features.Auth.Dtos;

public class UserForLoginDto : IDto
{
    public string UserName { get; set; }

    public string Password { get; set; }

    public string? AuthenticatorCode { get; set; }

    public UserForLoginDto()
    {
        UserName = string.Empty;
        Password = string.Empty;
    }

    public UserForLoginDto(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }
}
