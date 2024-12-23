using Grpc.Core;
using Mapster;
using MediatR;
using ScheduleService.Application.CQRS.ClassEntity.Commands.CreateClass;
using ScheduleService.Application.CQRS.ClassEntity.Commands.UpdateClass;

namespace ScheduleService.API.Services;

public class ClassService(IMediator mediator) : ScheduleService.ClassService.ClassServiceBase
{
    private readonly IMediator _mediator = mediator;

    public override async Task<CreateClassResponse> CreateClass(
        CreateClassRequest request,
        ServerCallContext context
    )
    {
        var command = request.Adapt<CreateClassCommand>();

        var @class = await _mediator.Send(command);

        return new CreateClassResponse { Class = @class.Adapt<Class>() };
    }

    public override async Task<UpdateClassResponse> UpdateClass(
        UpdateClassRequest request,
        ServerCallContext context
    )
    {
        var command = request.Adapt<UpdateClassCommand>();

        var @class = await _mediator.Send(command);

        return new UpdateClassResponse { Class = @class.Adapt<Class>() };
    }
}
