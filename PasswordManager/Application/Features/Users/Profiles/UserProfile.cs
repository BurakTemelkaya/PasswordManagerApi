using Application.Features.Users.Commands.UpdatePassword;
using AutoMapper;

namespace Application.Features.Users.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<Domain.Entities.User, UpdateUserPasswordCommand>().ReverseMap();
        CreateMap<Domain.Entities.User, UpdateUserPasswordResponse>().ReverseMap();

    }
}
