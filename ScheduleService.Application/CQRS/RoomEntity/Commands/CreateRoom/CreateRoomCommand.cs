using MediatR;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.RoomEntity.Commands.CreateRoom;

public record CreateRoomCommand(string Name, string? FullName) : IRequest<Room>;
