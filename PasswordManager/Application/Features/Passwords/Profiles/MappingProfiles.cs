using Application.Features.Passwords.Commands.Create;
using Application.Features.Passwords.Commands.Delete;
using Application.Features.Passwords.Commands.Update;
using Application.Features.Passwords.Dtos;
using AutoMapper;
using Core.Application.Responses;
using Core.Persistence.Paging;

namespace Application.Features.Passwords.Profiles;

public class MappingProfiles : Profile
{
	public MappingProfiles()
	{
		CreateMap<Domain.Entities.Password, CreatePasswordResponse>().ReverseMap();
		CreateMap<Domain.Entities.Password, CreatePasswordDto>().ReverseMap();
		CreateMap<Domain.Entities.Password, CreatePasswordCommand>().ReverseMap();

		CreateMap<Domain.Entities.Password, UpdatedPasswordResponse>().ReverseMap();
		CreateMap<Domain.Entities.Password, UpdatedPasswordDto>().ReverseMap();
		CreateMap<Domain.Entities.Password, UpdatedPasswordCommand>().ReverseMap();

		CreateMap<Domain.Entities.Password, GetByIdPasswordDto>().ReverseMap();

		CreateMap<Domain.Entities.Password, GetListPasswordDto>().ReverseMap();
		CreateMap<IPaginate<Domain.Entities.Password>, GetListResponse<GetListPasswordDto>>().ReverseMap();

		CreateMap<Domain.Entities.Password, DeletePasswordResponse>().ReverseMap();
	}
}
