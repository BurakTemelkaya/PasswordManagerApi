using Application.Features.Passwords.Constants;
using Application.Features.Passwords.Dtos;
using Application.Features.Users.Rules;
using Application.Services.Passwords;
using Application.Services.Users;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.CrossCuttingConcerns.Exception.Types;
using Core.Security.Constants;
using Core.Security.Hashing;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Commands.UpdatePassword;

public class UpdateUserPasswordCommand : IRequest<UpdateUserPasswordResponse>, ISecuredRequest
{
    public string[] Roles => [GeneralOperationClaims.User];

    public Guid? UserId { get; set; }
    public byte[] ExistPassword { get; set; }
    public byte[] NewPassword { get; set; }
    public ICollection<UpdatedPasswordDto> UpdatedPasswords { get; set; }

    public UpdateUserPasswordCommand()
    {
        ExistPassword = [];
        NewPassword = [];
        UpdatedPasswords = new HashSet<UpdatedPasswordDto>();
    }

    public UpdateUserPasswordCommand(Guid? userId, byte[] existPassword, byte[] newPassword, ICollection<UpdatedPasswordDto> updatedPasswords)
    {
        UserId = userId;
        ExistPassword = existPassword;
        NewPassword = newPassword;
        UpdatedPasswords = updatedPasswords;
    }

    public class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand, UpdateUserPasswordResponse>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;
        private readonly UserBusinessRules _userBusinessRules;

        public UpdateUserPasswordCommandHandler(IUserService userService, IMapper mapper
            , IPasswordService passwordService, UserBusinessRules userBusinessRules)
        {
            _userService = userService;
            _mapper = mapper;
            _passwordService = passwordService;
            _userBusinessRules = userBusinessRules;
        }

        public async Task<UpdateUserPasswordResponse> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.User? user = await _userService.GetAsync(
                predicate: u => u.Id.Equals(request.UserId),
                include: u => u.Include(u => u.Passwords),
                enableTracking: true,
                cancellationToken: cancellationToken
            );

            if (user == null)
            {
                throw new BusinessException("User not found.");
            }

            await _userBusinessRules.UserPasswordShouldBeMatched(user, request.ExistPassword);

            // Kullanıcının sahip olduğu tüm şifre ID'lerini bir HashSet'e alıyoruz (Hızlı arama için)
            var ownedPasswordIds = user.Passwords.Select(p => p.Id).ToHashSet();

            foreach (var updatedPassword in request.UpdatedPasswords)
            {
                // Eğer istekteki ID, kullanıcının sahip olduğu ID'ler arasında yoksa hata fırlat
                if (!ownedPasswordIds.Contains(updatedPassword.Id))
                {
                    throw new BusinessException(PasswordMessages.UserDoesNotMatch);
                }
            }

            HashingHelper.CreateMasterPasswordHash(
                request.NewPassword,
                hash: out byte[] passwordHash,
                salt: out byte[] passwordSalt
            );

            user!.MasterPasswordHash = passwordHash;
            user.MasterPasswordSalt = passwordSalt;

            _mapper.Map(request.UpdatedPasswords, user.Passwords);

            await _userService.UpdateAsync(user);

            UpdateUserPasswordResponse response = _mapper.Map<UpdateUserPasswordResponse>(user);

            return response;
        }
    }
}
