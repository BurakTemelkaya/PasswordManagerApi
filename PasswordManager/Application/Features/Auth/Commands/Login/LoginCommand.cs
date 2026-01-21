using Application.Features.Auth.Dtos;
using Application.Features.Auth.Rules;
using Application.Services.AuthService;
using Application.Services.Users;
using Core.Security.Hashing;
using Core.Security.JWT;
using Infrastructure.Caching;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.Features.Auth.Commands.Login;

public class LoginCommand : IRequest<LoggedResponse>
{
	public UserForLoginDto UserForLoginDto { get; set; }
	public string IpAddress { get; set; }

	public LoginCommand()
	{
		UserForLoginDto = null!;
		IpAddress = string.Empty;
	}

	public LoginCommand(UserForLoginDto userForLoginDto, string ipAddress)
	{
		UserForLoginDto = userForLoginDto;
		IpAddress = ipAddress;
	}

	public class LoginCommandHandler : IRequestHandler<LoginCommand, LoggedResponse>
	{
		private readonly AuthBusinessRules _authBusinessRules;
		//private readonly IAuthenticatorService _authenticatorService;
		private readonly IAuthService _authService;
		private readonly IUserService _userService;
		private readonly ICacheManager _cacheManager;
		private readonly IConfiguration _configuration;

		public LoginCommandHandler(
			IUserService userService,
			IAuthService authService,
			AuthBusinessRules authBusinessRules,
			ICacheManager cacheManager,
			IConfiguration configuration
			//IAuthenticatorService authenticatorService
		)
		{
			_userService = userService;
			_authService = authService;
			_authBusinessRules = authBusinessRules;
			_cacheManager = cacheManager;
			_configuration = configuration;
			//_authenticatorService = authenticatorService;
		}

		public async Task<LoggedResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
		{
            Domain.Entities.User? user = await _userService.GetAsync(
				predicate: u => u.UserName == request.UserForLoginDto.UserName,
				cancellationToken: cancellationToken
			);
			await _authBusinessRules.UserShouldBeExistsWhenSelected(user);
			await _authBusinessRules.UserPasswordShouldBeMatch(user!, request.UserForLoginDto.Password);

			LoggedResponse loggedResponse = new();

			AccessToken createdAccessToken = await _authService.CreateAccessToken(user);
			Domain.Entities.RefreshToken createdRefreshToken = await _authService.CreateRefreshToken(user, request.IpAddress);
			Domain.Entities.RefreshToken addedRefreshToken = await _authService.AddRefreshToken(createdRefreshToken);
			await _authService.DeleteOldRefreshTokens(user.Id);

			loggedResponse.AccessToken = createdAccessToken;
			loggedResponse.RefreshToken = addedRefreshToken;

            byte[] encryptionKey = HashingHelper.DeriveEncryptionKey(
				request.UserForLoginDto.Password, user.MasterPasswordSalt
			);
			
			// Client'ın kullanması için encryption key'i response'a ekle
			loggedResponse.EncryptionKey = Convert.ToBase64String(encryptionKey);

			return loggedResponse;
		}
	}
}
