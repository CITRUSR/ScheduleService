using MediatR;

namespace ScheduleService.API.Services;

public class CurrentWeekdayService(IMediator mediator)
    : ScheduleService.CurrentWeekdayService.CurrentWeekdayServiceBase
{
    private readonly IMediator _mediator = mediator;
}
