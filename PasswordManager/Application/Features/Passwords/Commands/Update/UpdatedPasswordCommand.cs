using Application.Features.Passwords.Constants;
using Application.Features.Passwords.Dtos;
using Application.Services.Passwords;
using Application.Services.Users;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.CrossCuttingConcerns.Exception.Types;
using Core.Security.Constants;
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
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;
        private readonly IUserService _userService;

        public UpdatePasswordCommandHandler(IMapper mapper, IPasswordService passwordService,IUserService userService)
        {
            _mapper = mapper;
            _passwordService = passwordService;
            _userService = userService;
        }
        public async Task<UpdatePasswordResponse> Handle(UpdatedPasswordCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.Password? password = await _passwordService.GetAsync(x => x.Id == request.UpdatedPasswordDto.Id);

            if (password == null)
            {
                throw new BusinessException(PasswordMessages.PasswordNotExist);
            }

            if (password.UserId != request.UpdatedPasswordDto.UserId)
            {
                throw new BusinessException(PasswordMessages.UserDoesNotMatch);
            }

            password = _mapper.Map(request.UpdatedPasswordDto, password);

            Domain.Entities.Password updatedPassword = await _passwordService.UpdateAsync(password);

            await _userService.UpdateVaultLastUpdatedDateAsync(updatedPassword.UserId);

            UpdatePasswordResponse updatedPasswordResponse = _mapper.Map<UpdatePasswordResponse>(updatedPassword);

            return updatedPasswordResponse;
        }
    }
}
