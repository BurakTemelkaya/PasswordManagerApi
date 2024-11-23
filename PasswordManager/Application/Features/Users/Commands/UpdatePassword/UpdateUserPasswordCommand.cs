using Application.Features.Auth.Rules;
using Application.Features.Passwords.Constants;
using Application.Features.Passwords.Rules;
using Application.Features.Users.Rules;
using Application.Services.Passwords;
using Application.Services.Users;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Persistence.Paging;
using Core.Security.Constants;
using Core.Security.Hashing;
using Core.Security.JWT;
using Domain.Entities;
using Infrastructure.Caching;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.Features.Users.Commands.UpdatePassword;

public class UpdateUserPasswordCommand : IRequest<UpdateUserPasswordResponse>, ISecuredRequest
{
    public string[] Roles => new string[] { GeneralOperationClaims.User };

    public Guid? UserId { get; set; }
    public string ExistPassword { get; set; }
    public string NewPassword { get; set; }

    public UpdateUserPasswordCommand()
    {
        ExistPassword = string.Empty;
        NewPassword = string.Empty;
    }

    public UpdateUserPasswordCommand(Guid? userId, string existPassword, string newPassword)
    {
        UserId = userId;
        ExistPassword = existPassword;
        NewPassword = newPassword;
    }

    public class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand, UpdateUserPasswordResponse>
    {
        private readonly IUserService _userService;
        private readonly UserBusinessRules _userBusinessRules;
        private readonly IMapper _mapper;
        private readonly ICacheManager _cacheManager;
        private readonly IPasswordService _passwordService;

        public UpdateUserPasswordCommandHandler(IUserService userService, UserBusinessRules userBusinessRules, IMapper mapper, ICacheManager cacheManager
            , IPasswordService passwordService)
        {
            _userService = userService;
            _userBusinessRules = userBusinessRules;
            _mapper = mapper;
            _cacheManager = cacheManager;
            _passwordService = passwordService;
        }

        public async Task<UpdateUserPasswordResponse> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.User? user = await _userService.GetAsync(
                predicate: u => u.Id.Equals(request.UserId),
                cancellationToken: cancellationToken
            );
            await _userBusinessRules.UserShouldBeExistsWhenSelected(user);
            await _userBusinessRules.UserPasswordShouldBeMatched(user!, request.ExistPassword);

            HashingHelper.CreateMasterPasswordHash(
                request.NewPassword,
                hash: out byte[] passwordHash,
                salt: out byte[] passwordSalt
            );

            user!.MasterPasswordHash = passwordHash;
            user.MasterPasswordSalt = passwordSalt;

            var newPasswordEncryptionKey = HashingHelper.DeriveEncryptionKey(
            request.NewPassword, passwordSalt
            );

            int passwordsCount = await _passwordService.GetPasswordCountByUserAsync(request.UserId!.Value);

            List<Domain.Entities.Password> updatedPasswords = new();

            string cacheKey = PasswordMessages.GetEncryptionCacheKey(request.UserId!.Value);
            var existEncryptionKey = _cacheManager.Get<byte[]>(cacheKey);

            for (int i = 0; i < (1 + passwordsCount / 2000); i++)
            {
                IPaginate<Domain.Entities.Password> passwords = await _passwordService.GetListAsync(x => x.UserId == request.UserId, size: 2000, index: i);

                if (passwords is null || passwords.Items is null || passwords.Items.Count == 0)
                {
                    break;
                }

                foreach (var password in passwords.Items)
                {
                    string existPassword = await AES256HashingHelper.DecryptBytesAsync(password.EncryptedPassword, existEncryptionKey);

                    Domain.Entities.Password updatePassword = password;

                    updatePassword.EncryptedPassword = await AES256HashingHelper.EncryptBytesAsync(existPassword, newPasswordEncryptionKey);

                    updatedPasswords.Add(updatePassword);
                }
                await _passwordService.UpdateRangeAsync(updatedPasswords);
                updatedPasswords.Clear();
            }

            await _userService.UpdateAsync(user);

            _cacheManager.Add(cacheKey, newPasswordEncryptionKey, 14400);

            UpdateUserPasswordResponse response = _mapper.Map<UpdateUserPasswordResponse>(user);

            return response;
        }
    }
}
