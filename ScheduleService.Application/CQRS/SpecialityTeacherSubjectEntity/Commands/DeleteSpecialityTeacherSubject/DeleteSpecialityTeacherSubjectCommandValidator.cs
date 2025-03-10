using FluentValidation;

namespace ScheduleService.Application.CQRS.SpecialityTeacherSubjectEntity.Commands.DeleteSpecialityTeacherSubject;

public class DeleteSpecialityTeacherSubjectCommandValidator
    : AbstractValidator<DeleteSpecialityTeacherSubjectCommand>
{
    public DeleteSpecialityTeacherSubjectCommandValidator()
    {
        RuleFor(x => x.SpecialityId).NotEqual(0);
        RuleFor(x => x.Course).NotEqual(0);
        RuleFor(x => x.Subgroup).NotEqual(0);
    }
}
