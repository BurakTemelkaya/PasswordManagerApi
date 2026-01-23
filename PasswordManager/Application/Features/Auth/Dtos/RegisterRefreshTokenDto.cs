namespace Application.Features.Auth.Dtos;

public class RefreshTokenForRegisterDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string CreatedByIp { get; set; }

    public RefreshTokenForRegisterDto()
    {
        Token = string.Empty;
        CreatedByIp = string.Empty;
    }

    public RefreshTokenForRegisterDto(Guid userId, string token, DateTime expirationDate, string createdByIp)
    {
        UserId = userId;
        Token = token;
        ExpirationDate = expirationDate;
        CreatedByIp = createdByIp;
    }
}