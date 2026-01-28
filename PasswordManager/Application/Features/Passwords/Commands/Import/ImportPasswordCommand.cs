using Application.Features.Passwords.Dtos;
using Application.Services.Passwords;
using Application.Services.Users;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Security.Constants;
using MediatR;

namespace Application.Features.Passwords.Commands.Import;

public class ImportPasswordCommand : IRequest<ImportPasswordResponse>, ISecuredRequest
{
    public ICollection<ImportPasswordDto> ImportPasswordsDto { get; set; }
    public Guid? UserId { get; set; }

    public string[] Roles => new[] { GeneralOperationClaims.User };

    public ImportPasswordCommand()
    {
        ImportPasswordsDto = null!;
    }

    public ImportPasswordCommand(ICollection<ImportPasswordDto> importPasswordsDto)
    {
        ImportPasswordsDto = importPasswordsDto;
    }

    public class ImportPasswordCommandHandler : IRequestHandler<ImportPasswordCommand, ImportPasswordResponse>
    {
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;
        private readonly IUserService _userService;

        public ImportPasswordCommandHandler(IMapper mapper, IPasswordService passwordService,IUserService userService)
        {
            _mapper = mapper;
            _passwordService = passwordService;
            _userService = userService;
        }
        public async Task<ImportPasswordResponse> Handle(ImportPasswordCommand request, CancellationToken cancellationToken)
        {
            List<Domain.Entities.Password> passwordToAdd = [];

            foreach (var importPassword in request.ImportPasswordsDto)
            {
                Domain.Entities.Password password = _mapper.Map<Domain.Entities.Password>(importPassword);

                password.UserId = request.UserId!.Value;

                passwordToAdd.Add(password);
            }

            ICollection<Domain.Entities.Password> addedPasswords = await _passwordService.AddRangeAsync(passwordToAdd);

            await _userService.UpdateVaultLastUpdatedDateAsync(request.UserId!.Value);

            ImportPasswordResponse importedPasswordsResponse = new()
            {
                AddedPasswordCount = addedPasswords.Count,
                AdditionDate = DateTime.UtcNow,
            };

            return importedPasswordsResponse;
        }
    }
}
