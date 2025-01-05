using FluentValidation;

namespace ScheduleService.Application.CQRS.ClassEntity.Commands.CreateClass;

public class CreateClassCommandValidator : AbstractValidator<CreateClassCommand>
{
    public CreateClassCommandValidator()
    {
        RuleFor(x => x.GroupId).NotEqual(0);
        RuleFor(x => x.SubjectId).NotEqual(0);
        RuleFor(x => x.WeekdayId).NotEqual(0);
        RuleFor(x => x.StartsAt).NotEqual(TimeSpan.MinValue).LessThan(x => x.EndsAt);
        RuleFor(x => x.EndsAt).NotEqual(TimeSpan.MinValue).GreaterThan(x => x.StartsAt);
        RuleFor(x => x.ChangeOn).NotEqual(DateTime.MinValue);
        RuleFor(x => x.TeachersIds).NotEmpty();
        RuleFor(x => x.RoomIds).NotEmpty();
    }
}
