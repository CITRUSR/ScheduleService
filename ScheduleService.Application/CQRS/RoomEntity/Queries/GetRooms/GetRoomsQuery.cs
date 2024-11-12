using MediatR;
using ScheduleService.Application.Common.Models;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.RoomEntity.Queries.GetRooms;

public record GetRoomsQuery(RoomFilter Filter, PaginationParameters PaginationParameters)
    : IRequest<PagedList<Room>>;
