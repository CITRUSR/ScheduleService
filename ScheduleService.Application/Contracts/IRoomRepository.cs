using ScheduleService.Application.Common.Models;
using ScheduleService.Application.CQRS.RoomEntity.Queries.GetRooms;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.Contracts;

public interface IRoomRepository
{
    Task<Room> InsertAsync(Room room);
    Task<Room?> GetByIdAsync(int id);
    Task<PagedList<Room>> GetAsync(RoomFilter filter, PaginationParameters paginationParameters);
    Task<Room?> UpdateAsync(Room room);
    Task<Room?> DeleteAsync(int id);
}
