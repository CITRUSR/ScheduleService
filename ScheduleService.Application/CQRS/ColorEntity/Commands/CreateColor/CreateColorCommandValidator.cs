using FluentValidation;

namespace ScheduleService.Application.CQRS.ColorEntity.Commands.CreateColor;

public class CreateColorCommandValidator : AbstractValidator<CreateColorCommand>
{
    public CreateColorCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(10);
    }
}
