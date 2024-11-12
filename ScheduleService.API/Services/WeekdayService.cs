using Grpc.Core;
using Mapster;
using MediatR;
using ScheduleService.Application.CQRS.WeekdayEntity.Queries.GetWeekdayById;
using ScheduleService.Application.CQRS.WeekdayEntity.Queries.GetWeekdays;

namespace ScheduleService.API.Services;

public class WeekdayService(IMediator mediator) : ScheduleService.WeekdayService.WeekdayServiceBase
{
    private readonly IMediator _mediator = mediator;

    public override async Task<GetWeekdayByIdResponse> GetWeekdayById(
        GetWeekdayByIdRequest request,
        ServerCallContext context
    )
    {
        var query = request.Adapt<GetWeekdayByIdQuery>();

        var weekday = await _mediator.Send(query);

        return new GetWeekdayByIdResponse { Weekday = weekday.Adapt<Weekday>() };
    }

    public override async Task<GetWeekdaysResponse> GetWeekdays(
        GetWeekdaysRequest request,
        ServerCallContext context
    )
    {
        var query = new GetWeekdaysQuery();

        var weekdays = await _mediator.Send(query);

        return new GetWeekdaysResponse { Weekdays = { weekdays.Adapt<List<Weekday>>() } };
    }
}
