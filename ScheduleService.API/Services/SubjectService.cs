using MediatR;

namespace ScheduleService.API.Services;

public class SubjectService(IMediator mediator) : ScheduleService.SubjectService.SubjectServiceBase
{
    private readonly IMediator _mediator = mediator;
}
