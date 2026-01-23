using Application.Features.Auth.Dtos;
using Core.Application.Responses;
using Core.Security.JWT;
using Domain.Entities;

namespace Application.Features.Auth.Commands.Register;

public class RegisteredResponse : IResponse
{
    public AccessTokenByRegisterDto AccessToken { get; set; }
    public RefreshTokenForRegisterDto RefreshToken { get; set; }
    public byte[] KdfSalt { get; set; }
    public int KdfIterations { get; set; }

    public RegisteredResponse()
    {
        AccessToken = null!;
        RefreshToken = null!;
        KdfSalt = [];
        KdfIterations = 0;
    }

    public RegisteredResponse(AccessTokenByRegisterDto accessToken, RefreshTokenForRegisterDto refreshToken, byte[] kdfSalt, int kdfIterations)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        KdfSalt = kdfSalt;
        KdfIterations = kdfIterations;
    }
}
