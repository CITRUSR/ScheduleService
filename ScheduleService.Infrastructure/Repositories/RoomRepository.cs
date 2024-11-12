using System.Text;
using Dapper;
using ScheduleService.Application.Common.Models;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.RoomEntity.Queries.GetRooms;
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

    public async Task<PagedList<Room>> GetAsync(
        RoomFilter filter,
        PaginationParameters paginationParameters
    )
    {
        using var connection = _dbContext.CreateConnection();

        StringBuilder queryBuilder = new StringBuilder();

        queryBuilder.Append(RoomQueries.GetRooms);

        if (!string.IsNullOrWhiteSpace(filter.SearchString))
        {
            queryBuilder.Append(@" WHERE LOWER(CONCAT(name, full_name)) LIKE LOWER(@SearchString)");
        }

        var searchString = $"%{filter.SearchString}%";

        var countQuery = queryBuilder
            .ToString()
            .Replace(RoomQueries.GetRooms, "SELECT COUNT(*) FROM rooms");

        string orderedCol = filter.FilterBy switch
        {
            RoomFilterState.Name => "name",
            RoomFilterState.FullName => "full_name",
        };

        queryBuilder.Append($" ORDER BY {orderedCol} {filter.OrderState}");

        queryBuilder.Append($" LIMIT {paginationParameters.PageSize}");

        queryBuilder.Append(
            $" OFFSET {(paginationParameters.Page - 1) * paginationParameters.PageSize}"
        );

        using var multy = await connection.QueryMultipleAsync(
            $"{countQuery}; {queryBuilder};",
            new { SearchString = searchString }
        );

        var totalCountTask = multy.ReadSingleOrDefaultAsync<int>();
        var roomsTask = multy.ReadAsync<Room>();

        await Task.WhenAll(roomsTask, totalCountTask);

        return new PagedList<Room>(
            [.. await roomsTask],
            await totalCountTask,
            paginationParameters.Page,
            paginationParameters.PageSize
        );
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
