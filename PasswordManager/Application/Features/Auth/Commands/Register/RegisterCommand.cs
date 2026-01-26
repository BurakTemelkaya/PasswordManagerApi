using Application.Features.Auth.Dtos;
using Application.Features.Auth.Rules;
using Application.Services.AuthService;
using Application.Services.OperationClaims;
using Application.Services.Users;
using AutoMapper;
using Core.Security.Constants;
using Core.Security.Hashing;
using Core.Security.JWT;
using Domain.Entities;
using MediatR;

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
		private readonly IOperationClaimService _operationClaimService;
		private readonly IMapper _mapper;

        public RegisterCommandHandler(
			IUserRepository userRepository,
			IAuthService authService,
			AuthBusinessRules authBusinessRules,
			IOperationClaimService operationCliamService,
			IMapper mapper
		)
		{
			_userRepository = userRepository;
			_authService = authService;
			_authBusinessRules = authBusinessRules;
			_operationClaimService = operationCliamService;
			_mapper = mapper;
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

            List<UserOperationClaim> addedClaims = [
				new()
				{
					OperationClaimId= userRole!.Id,
				}
			];

			User newUser =
				new()
				{
					Email = request.UserForRegisterDto.Email,
					UserName = request.UserForRegisterDto.UserName,
					MasterPasswordHash = passwordHash,
					MasterPasswordSalt = passwordSalt,
					UserOperationClaims = addedClaims,
					KdfSalt = request.UserForRegisterDto.KdfSalt,
					KdfIterations = request.UserForRegisterDto.KdfIterations
                };

			User createdUser = await _userRepository.AddAsync(newUser, cancellationToken);

			AccessToken createdAccessToken = await _authService.CreateAccessToken(createdUser);

			Domain.Entities.RefreshToken createdRefreshToken = await _authService.CreateRefreshToken(
				createdUser,
				request.IpAddress
			);
            Domain.Entities.RefreshToken addedRefreshToken = await _authService.AddRefreshToken(createdRefreshToken);

			RegisteredResponse registeredResponse = new()
			{
				AccessToken = _mapper.Map<AccessTokenByAuthDto>(createdAccessToken),
				RefreshToken = _mapper.Map<RefreshTokenForAuthDto>(addedRefreshToken),
				KdfIterations = createdUser.KdfIterations,
				KdfSalt = createdUser.KdfSalt
			};

			return registeredResponse;
		}
	}
}
