namespace Application.Features.Auth.Dtos;

public class RefreshTokenForAuthDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string CreatedByIp { get; set; }

    public RefreshTokenForAuthDto()
    {
        Token = string.Empty;
        CreatedByIp = string.Empty;
    }

    public RefreshTokenForAuthDto(Guid userId, string token, DateTime expirationDate, string createdByIp)
    {
        UserId = userId;
        Token = token;
        ExpirationDate = expirationDate;
        CreatedByIp = createdByIp;
    }
}