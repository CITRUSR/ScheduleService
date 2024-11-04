using FluentValidation;

namespace ScheduleService.Application.CQRS.ColorEntity.Commands.UpdateColor;

public class UpdateColorCommandValidator : AbstractValidator<UpdateColorCommand>
{
    public UpdateColorCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(10);
    }
}
