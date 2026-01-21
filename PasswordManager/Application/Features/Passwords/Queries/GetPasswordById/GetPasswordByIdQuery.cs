using Application.Features.Passwords.Dtos;
using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.CrossCuttingConcerns.Exception.Types;
using Core.Security.Constants;
using MediatR;

namespace Application.Features.Passwords.Queries.GetPasswordList;

public class GetPasswordByIdQuery : IRequest<GetByIdPasswordDto>, ISecuredRequest
{
	public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public string[] Roles => [GeneralOperationClaims.User];

	public class GetPasswordByIdQueryHandler : IRequestHandler<GetPasswordByIdQuery, GetByIdPasswordDto>
	{
		private readonly IPasswordRepository _passwordRepository;
		private readonly IMapper _mapper;

		public GetPasswordByIdQueryHandler(IPasswordRepository passwordRepository, IMapper mapper)
		{
			_passwordRepository = passwordRepository;
			_mapper = mapper;
		}

		public async Task<GetByIdPasswordDto> Handle(GetPasswordByIdQuery request, CancellationToken cancellationToken)
		{
			Domain.Entities.Password? password = await _passwordRepository
				.GetAsync(x => x.Id == request.Id,
				cancellationToken: cancellationToken);

			if (password == null)
			{
				throw new BusinessException("Parola bulunamadı.");
			}

			GetByIdPasswordDto getByIdPasswordDto = _mapper.Map<GetByIdPasswordDto>(password);

			return getByIdPasswordDto;
		}
	}
}
