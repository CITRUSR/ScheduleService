using MediatR;

namespace ScheduleService.API.Services;

public class RoomService(IMediator mediator) : ScheduleService.RoomService.RoomServiceBase
{
    private readonly IMediator _mediator = mediator;
}
