using FluentValidation;

namespace ScheduleService.Application.CQRS.CurrentWeekdayEntity.Commands.UpdateCurrentWeekday;

public class UpdateCurrentWeekdayCommandValidator : AbstractValidator<UpdateCurrentWeekdayCommand>
{
    public UpdateCurrentWeekdayCommandValidator()
    {
        RuleFor(x => x.Color).NotEmpty().MaximumLength(10);
        RuleFor(x => x.Interval).NotEqual(TimeSpan.Zero);
        RuleFor(x => x.UpdateTime).NotEqual(DateTime.MinValue);
    }
}
