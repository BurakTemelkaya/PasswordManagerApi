using Application.Features.Auth.Dtos;
using Core.Application.Responses;

namespace Application.Features.Auth.Commands.Register;

public class RegisteredResponse : IResponse
{
    public AccessTokenByAuthDto AccessToken { get; set; }
    public RefreshTokenForAuthDto RefreshToken { get; set; }
    public byte[] KdfSalt { get; set; }
    public int KdfIterations { get; set; }

    public RegisteredResponse()
    {
        AccessToken = null!;
        RefreshToken = null!;
        KdfSalt = [];
        KdfIterations = 0;
    }

    public RegisteredResponse(AccessTokenByAuthDto accessToken, RefreshTokenForAuthDto refreshToken, byte[] kdfSalt, int kdfIterations)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        KdfSalt = kdfSalt;
        KdfIterations = kdfIterations;
    }
}
