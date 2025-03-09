using FluentValidation;

namespace ScheduleService.Application.CQRS.SpecialityTeacherSubjectEntity.Queries.GetSpecialityTeacherSubjectById;

public class GetSpecialityTeacherSubjectByIdQueryValidator
    : AbstractValidator<GetSpecialityTeacherSubjectByIdQuery>
{
    public GetSpecialityTeacherSubjectByIdQueryValidator()
    {
        RuleFor(x => x.SpecialityId).NotEqual(0);
        RuleFor(x => x.Course).NotEqual(0);
        RuleFor(x => x.Subgroup).NotEqual(0);
    }
}
