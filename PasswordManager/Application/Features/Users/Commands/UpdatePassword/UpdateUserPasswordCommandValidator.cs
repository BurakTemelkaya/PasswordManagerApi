using FluentValidation;

namespace Application.Features.Users.Commands.UpdatePassword;

public class UpdateUserPasswordCommandValidator : AbstractValidator<UpdateUserPasswordCommand>
{
    public UpdateUserPasswordCommandValidator()
    {
        RuleFor(x => x.NewPassword).NotEmpty();
    }
}
