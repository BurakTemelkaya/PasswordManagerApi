using Application.Features.Passwords.Dtos;
using Application.Services.Passwords;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Requests;
using Core.Application.Responses;
using Core.Persistence.Paging;
using Core.Security.Constants;
using Domain.Entities;
using MediatR;

namespace Application.Features.Passwords.Queries.GetPasswordById;

public class GetPasswordListQuery : IRequest<GetListResponse<GetListPasswordDto>>, ISecuredRequest
{
	public PageRequest PageRequest { get; set; }

	public string[] Roles => new string[] { GeneralOperationClaims.User };

    public GetPasswordListQuery()
    {
		PageRequest = new PageRequest { PageIndex = 0, PageSize = 10 };
	}

    public GetPasswordListQuery(PageRequest pageRequest)
    {
        PageRequest = pageRequest;
    }

	public class GetPasswordListQueryHandler : IRequestHandler<GetPasswordListQuery, GetListResponse<GetListPasswordDto>>
	{
		private readonly IMapper _mapper;
		private readonly IPasswordService _passwordService;

		public GetPasswordListQueryHandler(IMapper mapper, IPasswordService passwordService)
		{
			_mapper = mapper;
			_passwordService = passwordService;
		}

		public async Task<GetListResponse<GetListPasswordDto>> Handle(GetPasswordListQuery request, CancellationToken cancellationToken)
		{
			IPaginate<Password> passwords = await _passwordService.GetListAsync(
				index: request.PageRequest.PageIndex,
				size: request.PageRequest.PageSize,
				enableTracking: false,
				cancellationToken: cancellationToken
			);

			GetListResponse<GetListPasswordDto> response = _mapper.Map<GetListResponse<GetListPasswordDto>>(passwords);
			return response;
		}
	}
}
