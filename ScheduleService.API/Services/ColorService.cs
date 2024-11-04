using Grpc.Core;
using Mapster;
using MediatR;
using ScheduleService.Application.CQRS.ColorEntity.Commands.CreateColor;

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
}
