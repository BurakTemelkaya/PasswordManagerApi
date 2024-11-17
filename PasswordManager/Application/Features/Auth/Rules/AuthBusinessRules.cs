using Application.Features.Auth.Constants;
using Application.Services.Users;
using Core.Application.Rules;
using Core.CrossCuttingConcerns.Exception.Types;
using Core.Localization.Abstraction;
using Core.Security.Hashing;
using Domain.Entities;

namespace Application.Features.Auth.Rules;

public class AuthBusinessRules : BaseBusinessRules
{
    private readonly IUserRepository _userRepository;
    private readonly ILocalizationService _localizationService;

    public AuthBusinessRules(IUserRepository userRepository, ILocalizationService localizationService)
    {
        _userRepository = userRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, AuthMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task UserShouldBeExistsWhenSelected(User? user)
    {
        if (user == null)
            await throwBusinessException(AuthMessages.UserDontExists);
    }

    public async Task RefreshTokenShouldBeExists(RefreshToken? refreshToken)
    {
        if (refreshToken == null)
            await throwBusinessException(AuthMessages.RefreshDontExists);
    }

    public async Task RefreshTokenShouldBeActive(RefreshToken refreshToken)
    {
        if (refreshToken.RevokedDate != null && DateTime.UtcNow >= refreshToken.ExpirationDate)
            await throwBusinessException(AuthMessages.InvalidRefreshToken);
    }

    public async Task UserEmailShouldBeNotExists(string email)
    {
        bool doesExists = await _userRepository.AnyAsync(predicate: u => u.Email == email);
        if (doesExists)
            await throwBusinessException(AuthMessages.UserMailAlreadyExists);
    }

    public async Task UserPasswordShouldBeMatch(User user, string password)
    {
        if (!HashingHelper.VerifyMasterPasswordHash(password, user!.MasterPasswordHash, user.MasterPasswordSalt))
            await throwBusinessException(AuthMessages.PasswordDontMatch);
    }
}
