using Application.Features.Passwords.Constants;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Application.Features.Passwords.Commands.Create;

public class CreatePasswordValidator : AbstractValidator<CreatePasswordCommand>
{
    public CreatePasswordValidator()
    {
        RuleFor(p=> p.CreatePasswordDto.Name).MinimumLength(3).NotEmpty();
		RuleFor(p => p.CreatePasswordDto.UserName).MaximumLength(50).NotEmpty();
		RuleFor(c => c.CreatePasswordDto.Password)
			.NotEmpty()
			.MinimumLength(6)
			.Must(PasswordRegex.StrongPassword)
			.WithMessage(
				"Password must contain at least one uppercase letter, one lowercase letter, one number and one special character."
			);
	}
}
