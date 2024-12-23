using FluentValidation;

namespace ScheduleService.Application.CQRS.ClassEntity.Commands.UpdateClass;

public class UpdateClassCommandValidator : AbstractValidator<UpdateClassCommand>
{
    public UpdateClassCommandValidator()
    {
        RuleFor(x => x.ClassId).NotEqual(0);
        RuleFor(x => x.GroupId).NotEqual(0);
        RuleFor(x => x.SubjectId).NotEqual(0);
        RuleFor(x => x.WeekdayId).NotEqual(0);
        RuleFor(x => x.StartsAt).NotEqual(TimeSpan.MinValue);
        RuleFor(x => x.EndsAt).NotEqual(TimeSpan.MinValue);
        RuleFor(x => x.ChangeOn).NotEqual(DateTime.MinValue);
        RuleFor(x => x.TeacherIds).NotEmpty();
        RuleFor(x => x.RoomIds).NotEmpty();
    }
}
