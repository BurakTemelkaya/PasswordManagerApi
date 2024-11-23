using Application.Features.Passwords.Constants;
using Application.Features.Passwords.Dtos;
using Application.Features.Passwords.Rules;
using Application.Services.Passwords;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Security.Constants;
using Core.Security.Hashing;
using Infrastructure.Caching;
using MediatR;

namespace Application.Features.Passwords.Commands.Update;

public class UpdatedPasswordCommand : IRequest<UpdatePasswordResponse>, ISecuredRequest
{
	public UpdatedPasswordDto UpdatedPasswordDto { get; set; }

	public string[] Roles => new[] { GeneralOperationClaims.User };

	public UpdatedPasswordCommand()
	{
		UpdatedPasswordDto = null!;
	}

	public UpdatedPasswordCommand(UpdatedPasswordDto updatedPasswordDto)
	{
		UpdatedPasswordDto = updatedPasswordDto;
	}

	public class UpdatePasswordCommandHandler : IRequestHandler<UpdatedPasswordCommand, UpdatePasswordResponse>
	{
		private readonly ICacheManager _cacheManager;
		private readonly IMapper _mapper;
		private readonly IPasswordService _passwordService;
		private readonly PasswordBusinessRules _passwordBusinessRules;

		public UpdatePasswordCommandHandler(ICacheManager cacheManager, IMapper mapper, IPasswordService passwordService
			, PasswordBusinessRules passwordBusinessRules)
		{
			_cacheManager = cacheManager;
			_mapper = mapper;
			_passwordService = passwordService;
			_passwordBusinessRules = passwordBusinessRules;
		}
		public async Task<UpdatePasswordResponse> Handle(UpdatedPasswordCommand request, CancellationToken cancellationToken)
		{
			Domain.Entities.Password? password = await _passwordService.GetAsync(x => x.Id == request.UpdatedPasswordDto.Id);

			await _passwordBusinessRules.PasswordNotExist(password);

			await _passwordBusinessRules.UserNotMatch(password!, request.UpdatedPasswordDto.UserId!.Value);

			password = _mapper.Map(request.UpdatedPasswordDto, password);

            string cacheKey = PasswordMessages.GetEncryptionCacheKey(request.UpdatedPasswordDto.UserId!.Value);
            byte[] encryptionKey = _cacheManager.Get<byte[]>(cacheKey);

			await _passwordBusinessRules.EncryptionKeyNotFound(encryptionKey);

			password!.EncryptedPassword = await AES256HashingHelper.EncryptBytesAsync(request.UpdatedPasswordDto.Password, encryptionKey);

			Domain.Entities.Password updatedPassword = await _passwordService.UpdateAsync(password);

			UpdatePasswordResponse updatedPasswordResponse = _mapper.Map<UpdatePasswordResponse>(updatedPassword);

			return updatedPasswordResponse;
		}
	}
}
