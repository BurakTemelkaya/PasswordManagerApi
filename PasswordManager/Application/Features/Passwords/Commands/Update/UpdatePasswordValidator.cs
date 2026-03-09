using FluentValidation;

namespace Application.Features.Passwords.Commands.Update;

public class UpdatePasswordValidator : AbstractValidator<UpdatedPasswordCommand>
{
    public UpdatePasswordValidator()
    {
        RuleFor(p => p.UpdatedPasswordDto.Id).NotEmpty();
        RuleFor(p => p.UpdatedPasswordDto.EncryptedName).NotEmpty();
        RuleFor(c => c.UpdatedPasswordDto.EncryptedPassword).NotEmpty();
        RuleFor(c => c.UpdatedPasswordDto.Iv).NotEmpty();
    }
}
