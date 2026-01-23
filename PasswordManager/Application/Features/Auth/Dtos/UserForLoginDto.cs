using Core.Application.Dtos;

namespace Application.Features.Auth.Dtos;

public class UserForLoginDto : IDto
{
    public string UserName { get; set; }

    public byte[] Password { get; set; }

    public string? AuthenticatorCode { get; set; }

    public UserForLoginDto()
    {
        UserName = string.Empty;
        Password = [];
    }

    public UserForLoginDto(string userName, byte[] password)
    {
        UserName = userName;
        Password = password;
    }
}
