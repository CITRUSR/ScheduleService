using FluentValidation;

namespace ScheduleService.Application.CQRS.SpecialityTeacherSubjectEntity.Commands.UpdateSpecialityTeacherSubject;

public class UpdateSpecialityTeacherSubjectCommandValidator
    : AbstractValidator<UpdateSpecialityTeacherSubjectCommand>
{
    public UpdateSpecialityTeacherSubjectCommandValidator()
    {
        RuleFor(x => x.SpecialityId).NotEqual(0);
        RuleFor(x => x.TeacherId).NotEqual(Guid.Empty);
        RuleFor(x => x.SubjectId).NotEqual(0);
        RuleFor(x => x.Course).NotEqual(0);
        RuleFor(x => x.Subgroup).NotEqual(0);
    }
}
