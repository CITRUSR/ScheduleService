using Grpc.Core;
using Mapster;
using MediatR;
using ScheduleService.Application.CQRS.ClassEntity.Commands.CreateClass;
using ScheduleService.Application.CQRS.ClassEntity.Commands.DeleteClass;
using ScheduleService.Application.CQRS.ClassEntity.Commands.UpdateClass;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClassById;

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

    public override async Task<GetClassByIdResponse> GetClassById(
        GetClassByIdRequest request,
        ServerCallContext context
    )
    {
        var query = request.Adapt<GetClassByIdQuery>();

        var @class = await _mediator.Send(query);

        return new GetClassByIdResponse { Class = @class.Adapt<Class>() };
    }

    public override async Task<DeleteClassResponse> DeleteClass(
        DeleteClassRequest request,
        ServerCallContext context
    )
    {
        var command = request.Adapt<DeleteClassCommand>();

        var @class = await _mediator.Send(command);

        return new DeleteClassResponse { Class = @class.Adapt<Class>() };
    }
}
