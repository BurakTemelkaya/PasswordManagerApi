using FluentValidation;

namespace Application.Features.Passwords.Commands.Create;

public class CreatePasswordValidator : AbstractValidator<CreatePasswordCommand>
{
    public CreatePasswordValidator()
    {
        RuleFor(p => p.CreatePasswordDto.EncryptedName).NotEmpty();
        RuleFor(p => p.CreatePasswordDto.EncryptedUserName).NotEmpty();
        RuleFor(c => c.CreatePasswordDto.EncryptedPassword).NotEmpty();
        RuleFor(c => c.CreatePasswordDto.Iv).NotEmpty();
    }
}
