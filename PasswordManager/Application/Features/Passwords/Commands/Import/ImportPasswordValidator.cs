using Application.Features.Passwords.Constants;
using Application.Features.Passwords.Dtos;
using FluentValidation;

namespace Application.Features.Passwords.Commands.Import;

public class ImportPasswordValidator : AbstractValidator<ImportPasswordDto>
{
    public ImportPasswordValidator()
    {
        RuleFor(ip=> ip.Name).MinimumLength(3).NotEmpty();
		RuleFor(ip => ip.Password)
			.NotEmpty()
			.MinimumLength(6)
			.Must(PasswordRegex.StrongPassword)
			.WithMessage(
				"Password must contain at least one uppercase letter, one lowercase letter, one number and one special character."
			);
	}
}
