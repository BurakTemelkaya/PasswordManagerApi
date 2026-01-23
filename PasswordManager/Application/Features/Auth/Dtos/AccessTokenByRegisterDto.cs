namespace Application.Features.Auth.Dtos;

public class AccessTokenByRegisterDto
{
    public string Token { get; set; }
    public DateTime ExpirationDate { get; set; }

    public AccessTokenByRegisterDto()
    {
        Token = string.Empty;
    }
}