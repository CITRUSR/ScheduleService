using FluentValidation;

namespace ScheduleService.Application.CQRS.SpecialityTeacherSubjectEntity.Commands.CreateSpecialityTeacherSubject;

public class CreateSpecialityTeacherSubjectCommandValidator
    : AbstractValidator<CreateSpecialityTeacherSubjectCommand>
{
    public CreateSpecialityTeacherSubjectCommandValidator()
    {
        RuleFor(x => x.SpecialityId).NotEqual(0);
        RuleFor(x => x.Course).NotEqual(0);
        RuleFor(x => x.SubGroup).NotEqual(0);
        RuleFor(x => x.TeacherId).NotEqual(Guid.Empty);
        RuleFor(x => x.SubjectId).NotEqual(0);
    }
}
