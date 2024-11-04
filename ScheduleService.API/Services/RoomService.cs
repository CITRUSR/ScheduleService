using Grpc.Core;
using Mapster;
using MediatR;
using ScheduleService.Application.CQRS.RoomEntity.Commands.CreateRoom;

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
}
