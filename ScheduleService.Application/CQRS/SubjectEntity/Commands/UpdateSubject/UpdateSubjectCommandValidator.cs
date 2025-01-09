using FluentValidation;

namespace ScheduleService.Application.CQRS.SubjectEntity.Commands.UpdateSubject;

public class UpdateSubjectCommandValidator : AbstractValidator<UpdateSubjectCommand>
{
    public UpdateSubjectCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(128);
        RuleFor(x => x.Abbreviation).MaximumLength(10);
    }
}
