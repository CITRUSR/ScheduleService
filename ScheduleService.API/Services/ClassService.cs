using MediatR;

namespace ScheduleService.API.Services;

public class ClassService(IMediator mediator) : ScheduleService.ClassService.ClassServiceBase
{
    private readonly IMediator _mediator = mediator;
}
