using FluentValidation;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.GetClassesOnCurrentDateForTeacher;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Teacher.GetClassesOnCurrentDateForTeacher;

public class GetClassesOnCurrentDateForTeacherQueryValidator
    : AbstractValidator<GetClassesOnCurrentDateForTeacherQuery>
{
    public GetClassesOnCurrentDateForTeacherQueryValidator()
    {
        RuleFor(x => x.TeacherId).NotEqual(Guid.Empty);
    }
}
