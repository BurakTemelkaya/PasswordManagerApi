using AutoMapper;
using Domain.Entities;

namespace Application.Features.Auth.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Core.Security.Entities.RefreshToken<Guid, Guid>, RefreshToken>().ReverseMap();

        //CreateMap<RefreshToken, RevokedTokenResponse>().ReverseMap();
    }
}
