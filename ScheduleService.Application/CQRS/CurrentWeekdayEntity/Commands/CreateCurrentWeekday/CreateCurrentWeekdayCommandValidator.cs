using FluentValidation;

namespace ScheduleService.Application.CQRS.CurrentWeekdayEntity.Commands.CreateCurrentWeekday;

public class CreateCurrentWeekdayCommandValidator : AbstractValidator<CreateCurrentWeekdayCommand>
{
    public CreateCurrentWeekdayCommandValidator()
    {
        RuleFor(x => x.Color).NotEmpty().MaximumLength(10);
        RuleFor(x => x.Interval).NotEqual(TimeSpan.Zero);
    }
}
