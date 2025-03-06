using System.Data;
using Dapper;
using ScheduleService.Application.Common.Models;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.RoomEntity.Queries.GetRooms;
using ScheduleService.Domain.Entities;
using ScheduleService.Infrastructure.Repositories.Sql;

namespace ScheduleService.Infrastructure.Repositories;

public class RoomRepository(IDbConnection dbConnection) : IRoomRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task<Room?> DeleteAsync(int id)
    {
        var room = await _dbConnection.QueryFirstOrDefaultAsync<Room>(
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
        string searchCondition = "";

        if (!string.IsNullOrWhiteSpace(filter.SearchString))
        {
            searchCondition = " WHERE LOWER(CONCAT(name, full_name)) LIKE LOWER(@SearchString)";
        }

        var searchString = $"%{filter.SearchString}%";

        string orderedCol = filter.FilterBy switch
        {
            RoomFilterState.Name => "name",
            RoomFilterState.FullName => "full_name",
        };

        var offset = (paginationParameters.Page - 1) * paginationParameters.PageSize;

        var query = string.Format(
            RoomQueries.GetRooms,
            searchCondition,
            orderedCol,
            filter.OrderState
        );

        var parameters = new DynamicParameters();

        parameters.Add("@SearchString", searchString);
        parameters.Add("@Limit", paginationParameters.PageSize);
        parameters.Add("@Offset", offset);

        int totalCount = 0;

        var rooms = await _dbConnection.QueryAsync<Room, long, Room>(
            query,
            (rooms, count) =>
            {
                totalCount = (int)count;
                return rooms;
            },
            parameters,
            splitOn: "totalCount"
        );

        return new PagedList<Room>(
            [.. rooms],
            totalCount,
            paginationParameters.Page,
            paginationParameters.PageSize
        );
    }

    public async Task<Room?> GetByIdAsync(int id)
    {
        var room = await _dbConnection.QueryFirstOrDefaultAsync<Room>(
            RoomQueries.GetRoomById,
            new { id }
        );

        return room;
    }

    public async Task<Room> InsertAsync(Room room)
    {
        var roomId = await _dbConnection.QuerySingleAsync<int>(
            RoomQueries.InsertRoom,
            new { room.Name, room.FullName }
        );

        room.Id = roomId;

        return room;
    }

    public async Task<Room?> UpdateAsync(Room room)
    {
        var affectedRows = await _dbConnection.ExecuteAsync(
            RoomQueries.UpdateRoom,
            new
            {
                Name = room.Name,
                FullName = room.FullName,
                Id = room.Id,
            }
        );

        return affectedRows == 1 ? room : null;
    }
}
