using Application.Features.Passwords.Rules;
using Application.Services.Passwords;
using Application.Services.Users;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Security.Constants;
using Domain.Entities;
using MediatR;

namespace Application.Features.Passwords.Commands.Delete;

public class DeletePasswordCommand : IRequest<DeletePasswordResponse>, ISecuredRequest
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public string[] Roles => [GeneralOperationClaims.Admin, GeneralOperationClaims.User];

    public class DeletePasswordCommandHandler : IRequestHandler<DeletePasswordCommand, DeletePasswordResponse>
    {
        private readonly IPasswordService _passwordService;
        private readonly PasswordBusinessRules _passwordBusinessRules;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public DeletePasswordCommandHandler(IPasswordService passwordService, PasswordBusinessRules passwordBusinessRules, IMapper mapper, IUserService userService)
        {
            _passwordService = passwordService;
            _passwordBusinessRules = passwordBusinessRules;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<DeletePasswordResponse> Handle(DeletePasswordCommand request, CancellationToken cancellationToken)
        {
            Password? password = await _passwordService.GetAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

            await _passwordBusinessRules.PasswordNotExist(password);

            await _passwordBusinessRules.UserNotMatch(password!, request.UserId!.Value);

            Password deletedPassword = await _passwordService.DeleteAsync(password!, permanent: true);

            await _userService.UpdateVaultLastUpdatedDateAsync(request.UserId!.Value);

            return _mapper.Map<DeletePasswordResponse>(deletedPassword);
        }
    }
}
