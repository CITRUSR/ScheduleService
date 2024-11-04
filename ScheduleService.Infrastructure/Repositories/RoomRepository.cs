using Dapper;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;
using ScheduleService.Infrastructure.Repositories.Sql;

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

    public async Task<Room> InsertAsync(Room room)
    {
        using var connection = _dbContext.CreateConnection();

        var roomId = await connection.QuerySingleAsync<int>(
            RoomQueries.InsertRoom,
            new { room.Name, room.FullName }
        );

        room.Id = roomId;

        return room;
    }

    public Task<Room> UpdateAsync(Room room)
    {
        throw new NotImplementedException();
    }
}
