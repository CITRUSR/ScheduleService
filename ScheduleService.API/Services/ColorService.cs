using Grpc.Core;
using Mapster;
using MediatR;
using ScheduleService.Application.CQRS.ColorEntity.Commands.CreateColor;
using ScheduleService.Application.CQRS.ColorEntity.Queries.GetColorById;

namespace ScheduleService.API.Services;

public class ColorService(IMediator mediator) : ScheduleService.ColorService.ColorServiceBase
{
    private IMediator _mediator = mediator;

    public override async Task<CreateColorResponse> CreateColor(
        CreateColorRequest request,
        ServerCallContext context
    )
    {
        var command = request.Adapt<CreateColorCommand>();

        var color = await _mediator.Send(command);

        return new CreateColorResponse { Color = color.Adapt<Color>() };
    }

    public override async Task<GetColorByIdResponse> GetColorById(
        GetColorByIdRequest request,
        ServerCallContext context
    )
    {
        var query = request.Adapt<GetColorByIdQuery>();

        var color = await _mediator.Send(query);

        return new GetColorByIdResponse { Color = color.Adapt<ColorViewModel>() };
    }
}
