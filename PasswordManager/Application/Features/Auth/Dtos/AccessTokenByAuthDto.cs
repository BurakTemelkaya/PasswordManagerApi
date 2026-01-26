namespace Application.Features.Auth.Dtos;

public class AccessTokenByAuthDto
{
    public string Token { get; set; }
    public DateTime ExpirationDate { get; set; }

    public AccessTokenByAuthDto()
    {
        Token = string.Empty;
    }
}