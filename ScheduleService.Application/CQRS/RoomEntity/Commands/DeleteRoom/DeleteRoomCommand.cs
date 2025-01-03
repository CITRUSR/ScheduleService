using MediatR;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.RoomEntity.Commands.DeleteRoom;

public record DeleteRoomCommand(int Id) : IRequest<Room>;
