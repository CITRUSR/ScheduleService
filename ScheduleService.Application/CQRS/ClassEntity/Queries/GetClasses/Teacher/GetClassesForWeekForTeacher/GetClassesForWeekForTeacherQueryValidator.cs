using FluentValidation;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Teacher.GetClassesForWeekForTeacher;

public class GetClassesForWeekForTeacherQueryValidator
    : AbstractValidator<GetClassesForWeekForTeacherQuery>
{
    public GetClassesForWeekForTeacherQueryValidator()
    {
        RuleFor(x => x.TeacherId).NotEqual(Guid.Empty);
    }
}
