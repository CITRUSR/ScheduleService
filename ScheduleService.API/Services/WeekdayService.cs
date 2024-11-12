using MediatR;

namespace ScheduleService.API.Services;

public class WeekdayService(IMediator mediator) : ScheduleService.WeekdayService.WeekdayServiceBase
{
    private readonly IMediator _mediator = mediator;
}
