using MediatR;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.RoomEntity.Queries.GetRoomById;

public record GetRoomByIdQuery(int Id) : IRequest<Room>;
