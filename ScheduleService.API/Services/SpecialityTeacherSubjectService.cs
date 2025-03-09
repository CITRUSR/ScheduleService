using MediatR;

namespace ScheduleService.API.Services;

public class SpecialityTeacherSubjectService(IMediator mediator)
    : ScheduleService.SpecialityTeacherSubjectService.SpecialityTeacherSubjectServiceBase
{
    private readonly IMediator _mediator = mediator;
}
