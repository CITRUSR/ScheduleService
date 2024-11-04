using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Infrastructure.Repositories;

public class RoomRepository(IDbContext dbContext) : IRoomRepository
{
    private readonly IDbContext _dbContext = dbContext;

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Room>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Room?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Room> InsertAsync(Room room)
    {
        throw new NotImplementedException();
    }

    public Task<Room> UpdateAsync(Room room)
    {
        throw new NotImplementedException();
    }
}
