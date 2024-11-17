using Application.Features.Auth.Dtos;
using Application.Features.Auth.Rules;
using Application.Services.AuthService;
using Application.Services.OperationClaims;
using Application.Services.UserOperationClaims;
using Application.Services.Users;
using Core.Security.Constants;
using Core.Security.Hashing;
using Core.Security.JWT;
using Domain.Entities;
using Infrastructure.Caching;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.Features.Auth.Commands.Register;

public class RegisterCommand : IRequest<RegisteredResponse>
{
	public UserForRegisterDto UserForRegisterDto { get; set; }
	public string IpAddress { get; set; }

	public RegisterCommand()
	{
		UserForRegisterDto = null!;
		IpAddress = string.Empty;
	}

	public RegisterCommand(UserForRegisterDto userForRegisterDto, string ipAddress)
	{
		UserForRegisterDto = userForRegisterDto;
		IpAddress = ipAddress;
	}

	public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisteredResponse>
	{
		private readonly IUserRepository _userRepository;
		private readonly IAuthService _authService;
		private readonly AuthBusinessRules _authBusinessRules;
		private readonly ICacheManager _cacheManager;
		private readonly IConfiguration _configuration;
		private readonly IUserOperationClaimService _userOperationClaimService;
		private readonly IOperationClaimService _operationClaimService;

		public RegisterCommandHandler(
			IUserRepository userRepository,
			IAuthService authService,
			AuthBusinessRules authBusinessRules,
			ICacheManager cacheManager,
			IConfiguration configuration,
			IUserOperationClaimService userOperationClaimService,
			IOperationClaimService operationCliamService

		)
		{
			_userRepository = userRepository;
			_authService = authService;
			_authBusinessRules = authBusinessRules;
			_cacheManager = cacheManager;
			_configuration = configuration;
			_userOperationClaimService = userOperationClaimService;
			_operationClaimService = operationCliamService;
		}

		public async Task<RegisteredResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
		{
			await _authBusinessRules.UserEmailShouldBeNotExists(request.UserForRegisterDto.Email);

			HashingHelper.CreateMasterPasswordHash(
				request.UserForRegisterDto.Password,
				hash: out byte[] passwordHash,
				salt: out byte[] passwordSalt
			);

			OperationClaim? userRole = await _operationClaimService.GetAsync(x => x.Name == GeneralOperationClaims.User
			, cancellationToken: cancellationToken);

			List<UserOperationClaim> addedClaims = new(){
				new()
				{
					OperationClaimId= userRole!.Id,
				}
			};

			User newUser =
				new()
				{
					Email = request.UserForRegisterDto.Email,
					UserName = request.UserForRegisterDto.UserName,
					MasterPasswordHash = passwordHash,
					MasterPasswordSalt = passwordSalt,
					UserOperationClaims = addedClaims
				};

			User createdUser = await _userRepository.AddAsync(newUser, cancellationToken);

			AccessToken createdAccessToken = await _authService.CreateAccessToken(createdUser);

			RefreshToken createdRefreshToken = await _authService.CreateRefreshToken(
				createdUser,
				request.IpAddress
			);
			RefreshToken addedRefreshToken = await _authService.AddRefreshToken(createdRefreshToken);

			RegisteredResponse registeredResponse = new() { AccessToken = createdAccessToken, RefreshToken = addedRefreshToken };



			var encryptionKey = HashingHelper.DeriveEncryptionKey(
			request.UserForRegisterDto.Password, createdUser.MasterPasswordSalt
			);

			string cacheKey = $"EncryptionKey_{createdUser.Id}";

			TokenOptions? tokenConfiguration = _configuration.GetSection("TokenOptions").Get<TokenOptions>();

			_cacheManager.Add(cacheKey, encryptionKey, tokenConfiguration.AccessTokenExpiration);

			return registeredResponse;
		}
	}
}
