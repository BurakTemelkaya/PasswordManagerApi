using Application.Features.Auth.Dtos;
using AutoMapper;

namespace Application.Features.Auth.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Core.Security.Entities.RefreshToken<Guid, Guid>, Domain.Entities.RefreshToken>().ReverseMap();

        CreateMap<Domain.Entities.RefreshToken, RefreshTokenForRegisterDto>().ReverseMap();
        CreateMap<Core.Security.JWT.AccessToken, AccessTokenByRegisterDto>().ReverseMap();
        CreateMap<Core.Security.Entities.RefreshToken<Guid, Guid>, Domain.Entities.RefreshToken>().ReverseMap();

        //CreateMap<RefreshToken, RevokedTokenResponse>().ReverseMap();
    }
}
