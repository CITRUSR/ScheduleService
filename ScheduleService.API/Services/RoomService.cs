using Grpc.Core;
using Mapster;
using MediatR;
using ScheduleService.Application.CQRS.RoomEntity.Commands.CreateRoom;
using ScheduleService.Application.CQRS.RoomEntity.Commands.UpdateRoom;
using ScheduleService.Application.CQRS.RoomEntity.Queries.GetRoomById;

namespace ScheduleService.API.Services;

public class RoomService(IMediator mediator) : ScheduleService.RoomService.RoomServiceBase
{
    private readonly IMediator _mediator = mediator;

    public override async Task<CreateRoomResponse> CreateRoom(
        CreateRoomRequest request,
        ServerCallContext context
    )
    {
        var command = request.Adapt<CreateRoomCommand>();

        var room = await _mediator.Send(command);

        return new CreateRoomResponse { Room = room.Adapt<Room>() };
    }

    public override async Task<GetRoomByIdResponse> GetRoomById(
        GetRoomByIdRequest request,
        ServerCallContext context
    )
    {
        var query = request.Adapt<GetRoomByIdQuery>();

        var room = await _mediator.Send(query);

        return new GetRoomByIdResponse { Room = room.Adapt<Room>() };
    }

    public override async Task<UpdateRoomResponse> UpdateRoom(
        UpdateRoomRequest request,
        ServerCallContext context
    )
    {
        var command = request.Adapt<UpdateRoomCommand>();

        var room = await _mediator.Send(command);

        return new UpdateRoomResponse { Room = room.Adapt<Room>() };
    }
}
