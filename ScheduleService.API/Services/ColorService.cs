using MediatR;

namespace ScheduleService.API.Services;

public class ColorService(IMediator mediator) : ScheduleService.ColorService.ColorServiceBase
{
    private IMediator _mediator = mediator;
}
