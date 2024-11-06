using Dapper;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;
using ScheduleService.Infrastructure.Repositories.Sql;

namespace ScheduleService.Infrastructure.Repositories;

public class RoomRepository(IDbContext dbContext) : IRoomRepository
{
    private readonly IDbContext _dbContext = dbContext;

    public async Task<Room?> DeleteAsync(int id)
    {
        using var connection = _dbContext.CreateConnection();

        var room = await connection.QueryFirstOrDefaultAsync<Room>(
            RoomQueries.DeleteRoom,
            new { id }
        );

        return room;
    }

    public Task<List<Room>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Room?> GetByIdAsync(int id)
    {
        using var connection = _dbContext.CreateConnection();

        var room = await connection.QueryFirstOrDefaultAsync<Room>(
            RoomQueries.GetRoomById,
            new { id }
        );

        return room;
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

    public async Task<Room?> UpdateAsync(Room room)
    {
        using var connection = _dbContext.CreateConnection();

        var affectedRows = await connection.ExecuteAsync(
            RoomQueries.UpdateRoom,
            new
            {
                Name = room.Name,
                FullName = room.FullName,
                Id = room.Id
            }
        );

        return affectedRows == 1 ? room : null;
    }
}
