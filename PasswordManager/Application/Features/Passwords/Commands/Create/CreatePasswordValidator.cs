using FluentValidation;

namespace Application.Features.Passwords.Commands.Create;

public class CreatePasswordValidator : AbstractValidator<CreatePasswordCommand>
{
    public CreatePasswordValidator()
    {
        RuleFor(p=> p.CreatePasswordDto.Name).MinimumLength(3).NotEmpty();
        RuleFor(p=> p.CreatePasswordDto.Password).MinimumLength(6).NotEmpty();
    }
}
