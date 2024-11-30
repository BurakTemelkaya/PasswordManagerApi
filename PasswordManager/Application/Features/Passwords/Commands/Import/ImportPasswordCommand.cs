using Application.Features.Passwords.Dtos;
using Application.Features.Passwords.Rules;
using Application.Services.Passwords;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Security.Constants;
using Core.Security.Hashing;
using Infrastructure.Caching;
using MediatR;
using Application.Features.Passwords.Constants;

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
        private readonly ICacheManager _cacheManager;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;
        private readonly PasswordBusinessRules _passwordBusinessRules;

        public ImportPasswordCommandHandler(ICacheManager cacheManager, IMapper mapper, IPasswordService passwordService
            , PasswordBusinessRules passwordBusinessRules)
        {
            _cacheManager = cacheManager;
            _mapper = mapper;
            _passwordService = passwordService;
            _passwordBusinessRules = passwordBusinessRules;
        }
        public async Task<ImportPasswordResponse> Handle(ImportPasswordCommand request, CancellationToken cancellationToken)
        {
            string cacheKey = PasswordMessages.GetEncryptionCacheKey(request.UserId!.Value);
            byte[] encryptionKey = _cacheManager.Get<byte[]>(cacheKey);

            await _passwordBusinessRules.EncryptionKeyNotFound(encryptionKey);

            List<Domain.Entities.Password> passwordToAdd = new();

            foreach (var importPassword in request.ImportPasswordsDto)
            {
                Domain.Entities.Password password = _mapper.Map<Domain.Entities.Password>(importPassword);

                password.UserId = request.UserId.Value;

                password.EncryptedPassword = await AES256HashingHelper.EncryptBytesAsync(importPassword.Password, encryptionKey);

                passwordToAdd.Add(password);
            }

            ICollection<Domain.Entities.Password> addedPasswords = await _passwordService.AddRangeAsync(passwordToAdd);

            ImportPasswordResponse importedPasswordsResponse = new()
            {
                AddedPasswordCount = addedPasswords.Count,
                AdditionDate = DateTime.UtcNow,
            };

            return importedPasswordsResponse;
        }
    }
}
