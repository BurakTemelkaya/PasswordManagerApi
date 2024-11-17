using Application.Features.Auth.Rules;
using Application.Services.AuthService;
using Application.Services.Users;
using Core.Security.Hashing;
using Core.Security.JWT;
using Domain.Entities;
using MediatR;

namespace Application.Features.Auth.Commands.Register;

public class RegisterCommand : IRequest<RegisteredResponse>
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string IpAddress { get; set; }

    public RegisterCommand()
    {
        UserName = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
        IpAddress = string.Empty;
    }

    public RegisterCommand(string userName, string email, string password, string ipAddress)
    {
        UserName = userName;
        Email = email;
        Password = password;
        IpAddress = ipAddress;
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisteredResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        //private readonly AuthBusinessRules _authBusinessRules;

        public RegisterCommandHandler(
            IUserRepository userRepository,
            IAuthService authService,
            AuthBusinessRules authBusinessRules
        )
        {
            _userRepository = userRepository;
            _authService = authService;
            //_authBusinessRules = authBusinessRules;
        }

        public async Task<RegisteredResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            //await _authBusinessRules.UserEmailShouldBeNotExists(request.Email);

            HashingHelper.CreateMasterPasswordHash(
                request.Password,
                hash: out byte[] passwordHash,
                salt: out byte[] passwordSalt
            );

            User newUser =
                new()
                {
                    Email = request.Email,
                    UserName = request.UserName,
                    MasterPasswordHash = passwordHash,
                    MasterPasswordSalt = passwordSalt,
                };

            User createdUser = await _userRepository.AddAsync(newUser);

            AccessToken createdAccessToken = await _authService.CreateAccessToken(createdUser);

            RefreshToken createdRefreshToken = await _authService.CreateRefreshToken(
                createdUser,
                request.IpAddress
            );
            RefreshToken addedRefreshToken = await _authService.AddRefreshToken(createdRefreshToken);

            RegisteredResponse registeredResponse = new() { AccessToken = createdAccessToken, RefreshToken = addedRefreshToken };
            return registeredResponse;
        }
    }
}
