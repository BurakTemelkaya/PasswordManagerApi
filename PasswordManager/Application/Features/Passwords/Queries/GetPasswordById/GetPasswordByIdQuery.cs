using Application.Features.Passwords.Dtos;
using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Security.Constants;
using Core.Security.Hashing;
using Infrastructure.Caching;
using MediatR;

namespace Application.Features.Passwords.Queries.GetPasswordById;

public class GetPasswordByIdQuery : IRequest<GetByIdPasswordDto>, ISecuredRequest
{
	public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public string[] Roles => new string[] { GeneralOperationClaims.User };

	public class GetPasswordByIdQueryHandler : IRequestHandler<GetPasswordByIdQuery, GetByIdPasswordDto>
	{
		private readonly IPasswordRepository _passwordRepository;
		private readonly IMapper _mapper;
		private readonly ICacheManager _cacheManager;

		public GetPasswordByIdQueryHandler(IPasswordRepository passwordRepository, IMapper mapper, ICacheManager cacheManager)
		{
			_passwordRepository = passwordRepository;
			_mapper = mapper;
			_cacheManager = cacheManager;
		}

		public async Task<GetByIdPasswordDto> Handle(GetPasswordByIdQuery request, CancellationToken cancellationToken)
		{
			Domain.Entities.Password? password = await _passwordRepository
				.GetAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

			GetByIdPasswordDto getByIdPasswordDto = _mapper.Map<GetByIdPasswordDto>(password);

			string cacheKey = $"EncryptionKey_{request.UserId}";
			var encryptionKey = _cacheManager.Get<byte[]>(cacheKey);

			if (encryptionKey == null)
			{
				throw new Exception("Şifreleme anahtarı bulunamadı. Oturum süresi dolmuş olabilir.");
			}

			getByIdPasswordDto.Password = await AES256HashingHelper.DecryptBytesAsync(password.EncryptedPassword, encryptionKey);

			return getByIdPasswordDto;
		}
	}
}
