using Grpc.Core;
using Mapster;
using MediatR;
using ScheduleService.Application.CQRS.CurrentWeekdayEntity.Commands.UpdateCurrentWeekday;
using ScheduleService.Application.CQRS.CurrentWeekdayEntity.Queries.GetCurrentWeekday;

namespace ScheduleService.API.Services;

public class CurrentWeekdayService(IMediator mediator)
    : ScheduleService.CurrentWeekdayService.CurrentWeekdayServiceBase
{
    private readonly IMediator _mediator = mediator;

    public override async Task<GetCurrentWeekdayResponse> GetCurrentWeekday(
        GetCurrentWeekdayRequest request,
        ServerCallContext context
    )
    {
        var query = new GetCurrentWeekdayQuery();

        var currentWeekday = await _mediator.Send(query);

        return new GetCurrentWeekdayResponse
        {
            CurrentWeekday = currentWeekday.Adapt<CurrentWeekday>()
        };
    }

    public override async Task<UpdateCurrentWeekdayResponse> UpdateCurrentWeekday(
        UpdateCurrentWeekdayRequest request,
        ServerCallContext context
    )
    {
        var command = request.Adapt<UpdateCurrentWeekdayCommand>();

        var currentWeekday = await _mediator.Send(command);

        return new UpdateCurrentWeekdayResponse
        {
            CurrentWeekday = currentWeekday.Adapt<CurrentWeekday>()
        };
    }
}
