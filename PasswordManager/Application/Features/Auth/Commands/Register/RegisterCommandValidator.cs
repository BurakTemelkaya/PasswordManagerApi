using Application.Features.Passwords.Constants;
using FluentValidation;

namespace Application.Features.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(c=> c.UserForRegisterDto.UserName).MinimumLength(3).NotEmpty();
        RuleFor(c => c.UserForRegisterDto.Email).NotEmpty().EmailAddress();
        RuleFor(c => c.UserForRegisterDto.Password)
            .NotEmpty()
            .MinimumLength(6)
            .Must(PasswordRegex.StrongPassword)
            .WithMessage(
                "Password must contain at least one uppercase letter, one lowercase letter, one number and one special character."
            );
    }
}
