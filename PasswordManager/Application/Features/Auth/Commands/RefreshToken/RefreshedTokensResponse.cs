using Application.Features.Auth.Dtos;

namespace Application.Features.Auth.Commands.RefreshToken;

public class RefreshedTokensResponse
{
    public AccessTokenByAuthDto AccessToken { get; set; }
    public RefreshTokenForAuthDto RefreshToken { get; set; }

    public RefreshedTokensResponse()
    {
        AccessToken = null!;
        RefreshToken = null!;
    }

    public RefreshedTokensResponse(AccessTokenByAuthDto accessToken, RefreshTokenForAuthDto refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}
