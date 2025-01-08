using FluentValidation;

namespace ScheduleService.Application.CQRS.SubjectEntity.Commands.CreateSubject;

public class CreateSubjectCommandValidator : AbstractValidator<CreateSubjectCommand>
{
    public CreateSubjectCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(128);
        RuleFor(x => x.Abbreviation).MaximumLength(10);
    }
}
