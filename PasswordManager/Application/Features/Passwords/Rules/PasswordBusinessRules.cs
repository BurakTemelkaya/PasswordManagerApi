using Core.Application.Rules;
using Core.Localization.Abstraction;
using Application.Services.Repositories;
using Core.CrossCuttingConcerns.Exception.Types;
using Application.Features.Passwords.Constants;
using Domain.Entities;

namespace Application.Features.Passwords.Rules;

public class PasswordBusinessRules : BaseBusinessRules
{
	private readonly ILocalizationService _localizationService;
	private readonly IPasswordRepository _passwordRepository;

	public PasswordBusinessRules(ILocalizationService localizationService, IPasswordRepository passwordRepository)
	{
		_localizationService = localizationService;
		_passwordRepository = passwordRepository;
	}

	private async Task throwBusinessException(string messageKey)
	{
		string message = await _localizationService.GetLocalizedAsync(messageKey, PasswordMessages.SectionName);
		throw new BusinessException(message);
	}

	public async Task UserNotMatch(Password password, Guid userId)
	{
		if (password.UserId != userId)
		{
			await throwBusinessException(PasswordMessages.UserDoesNotMatch);
		}
	}

	public async Task PasswordNotExist(Password? password)
	{
		if (password is null)
		{
			await throwBusinessException(PasswordMessages.PasswordNotExist);
		}
	}

	public async Task EncryptionKeyNotFound(byte[] encryptionKey)
	{
		if (encryptionKey is null)
		{
			await throwBusinessException(PasswordMessages.EncryptionKeyNotFound);
		}
	}

}
