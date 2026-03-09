using FluentValidation;

namespace Application.Features.Auth.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(c => c.UserForLoginDto.UserName).NotEmpty().MinimumLength(3);
        RuleFor(c => c.UserForLoginDto.Password).NotEmpty();
    }
}
