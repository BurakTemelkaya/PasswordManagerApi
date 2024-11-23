using Application.Features.Auth.Commands.Login;
using Application.Features.Passwords.Dtos;
using Application.Features.Passwords.Rules;
using Application.Services.Passwords;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Security.Constants;
using Core.Security.Hashing;
using Infrastructure.Caching;
using MediatR;
using Application.Features.Passwords.Constants;

namespace Application.Features.Passwords.Commands.Create;

public class CreatePasswordCommand : IRequest<CreatePasswordResponse>, ISecuredRequest
{
	public CreatePasswordDto CreatePasswordDto { get; set; }

	public string[] Roles => new[] { GeneralOperationClaims.User };

	public CreatePasswordCommand()
	{
		CreatePasswordDto = null!;
	}

	public CreatePasswordCommand(CreatePasswordDto createPasswordDto)
	{
		CreatePasswordDto = createPasswordDto;
	}

	public class CreatePasswordCommandHandler : IRequestHandler<CreatePasswordCommand, CreatePasswordResponse>
	{
		private readonly ICacheManager _cacheManager;
		private readonly IMapper _mapper;
		private readonly IPasswordService _passwordService;
		private readonly PasswordBusinessRules _passwordBusinessRules;

		public CreatePasswordCommandHandler(ICacheManager cacheManager, IMapper mapper, IPasswordService passwordService
			,PasswordBusinessRules passwordBusinessRules)
		{
			_cacheManager = cacheManager;
			_mapper = mapper;
			_passwordService = passwordService;
			_passwordBusinessRules = passwordBusinessRules;
		}
		public async Task<CreatePasswordResponse> Handle(CreatePasswordCommand request, CancellationToken cancellationToken)
		{
			Domain.Entities.Password password = _mapper.Map<Domain.Entities.Password>(request.CreatePasswordDto);

			string cacheKey = PasswordMessages.GetEncryptionCacheKey(request.CreatePasswordDto.UserId!.Value);
			byte[] encryptionKey = _cacheManager.Get<byte[]>(cacheKey);

			await _passwordBusinessRules.EncryptionKeyNotFound(encryptionKey);

			password.EncryptedPassword = await AES256HashingHelper.EncryptBytesAsync(request.CreatePasswordDto.Password, encryptionKey);

			Domain.Entities.Password addedPassword = await _passwordService.AddAsync(password);

			CreatePasswordResponse createPasswordResponse = _mapper.Map<CreatePasswordResponse>(addedPassword);

			return createPasswordResponse;
		}
	}
}
