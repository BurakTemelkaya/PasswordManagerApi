using Application.Features.Passwords.Dtos;
using FluentValidation;

namespace Application.Features.Passwords.Commands.Import;

public class ImportPasswordValidator : AbstractValidator<ImportPasswordDto>
{
    public ImportPasswordValidator()
    {
        RuleFor(ip => ip.EncryptedName).NotEmpty();
        RuleFor(ip => ip.EncryptedPassword).NotEmpty();
        RuleFor(ip => ip.Iv).NotEmpty();

    }
}
