using MediatR;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.RoomEntity.Commands.UpdateRoom;

public record UpdateRoomCommand(int Id, string Name, string? FullName) : IRequest<Room>;
