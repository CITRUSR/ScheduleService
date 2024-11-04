using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.Contracts;

public interface IRoomRepository
{
    Task<Room> InsertAsync(Room room);
    Task<Room?> GetByIdAsync(int id);
    Task<List<Room>> GetAllAsync();
    Task<Room> UpdateAsync(Room room);
    Task DeleteAsync(int id);
}
