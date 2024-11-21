using Application.Features.Passwords.Constants;
using FluentValidation;

namespace Application.Features.Passwords.Commands.Update;

public class UpdatePasswordValidator : AbstractValidator<UpdatedPasswordCommand>
{
    public UpdatePasswordValidator()
    {
        RuleFor(p=> p.UpdatedPasswordDto.Id).NotEmpty();
        RuleFor(p=> p.UpdatedPasswordDto.Name).MinimumLength(3).NotEmpty();
		RuleFor(c => c.UpdatedPasswordDto.Password)
			.NotEmpty()
			.MinimumLength(6)
			.Must(PasswordRegex.StrongPassword)
			.WithMessage(
				"Password must contain at least one uppercase letter, one lowercase letter, one number and one special character."
			);
	}
}
