using Grpc.Core;
using Mapster;
using MediatR;
using ScheduleService.Application.CQRS.WeekdayEntity.Queries.GetWeekdayById;

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
}
