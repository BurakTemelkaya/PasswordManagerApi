using Application.Features.Passwords.Constants;
using FluentValidation;

namespace Application.Features.Users.Commands.UpdatePassword;

public class UpdateUserPasswordCommandValidator : AbstractValidator<UpdateUserPasswordCommand>
{
    public UpdateUserPasswordCommandValidator()
    {
        RuleFor(x => x.NewPassword).NotEmpty()
            .MinimumLength(6)
            .Must(PasswordRegex.StrongPassword)
            .WithMessage(
                "Password must contain at least one uppercase letter, one lowercase letter, one number and one special character."
            );
    }
}
