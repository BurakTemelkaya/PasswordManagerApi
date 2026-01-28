using Application.Features.Passwords.Dtos;
using Application.Services.Passwords;
using Application.Services.Users;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Security.Constants;
using MediatR;

namespace Application.Features.Passwords.Commands.Create;

public class CreatePasswordCommand : IRequest<CreatePasswordResponse>, ISecuredRequest
{
    public CreatePasswordDto CreatePasswordDto { get; set; }

    public string[] Roles => [GeneralOperationClaims.User];

    public CreatePasswordCommand()
    {
        CreatePasswordDto = null!;
    }

    public CreatePasswordCommand(CreatePasswordDto createPasswordDto)
    {
        CreatePasswordDto = createPasswordDto;
    }

    public class CreatePasswordCommandHandler : IRequestHandler<CreatePasswordCommand, CreatePasswordResponse>
    {
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;
        private readonly IUserService _userService;

        public CreatePasswordCommandHandler(IMapper mapper, IPasswordService passwordService, IUserService userService)
        {
            _mapper = mapper;
            _passwordService = passwordService;
            _userService = userService;
        }
        public async Task<CreatePasswordResponse> Handle(CreatePasswordCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.Password password = _mapper.Map<Domain.Entities.Password>(request.CreatePasswordDto);

            Domain.Entities.Password addedPassword = await _passwordService.AddAsync(password);

            await _userService.UpdateVaultLastUpdatedDateAsync(password.UserId);

            CreatePasswordResponse createPasswordResponse = _mapper.Map<CreatePasswordResponse>(addedPassword);

            return createPasswordResponse;
        }
    }
}
