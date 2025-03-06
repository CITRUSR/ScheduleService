using System.Data;
using Dapper;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;
using ScheduleService.Infrastructure.Repositories.Sql;

namespace ScheduleService.Infrastructure.Repositories;

public class WeekdayRepository(IDbConnection dbConnection) : IWeekdayRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task<List<Weekday>> GetAllAsync()
    {
        var weekdays = await _dbConnection.QueryAsync<Weekday>(WeekdayQueries.GetAllWeekdays);

        return [.. weekdays];
    }

    public async Task<Weekday?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();

        parameters.Add("WeekdayId", id);

        var weekday = await _dbConnection.QueryFirstOrDefaultAsync<Weekday>(
            WeekdayQueries.GetWeekdayById,
            parameters
        );

        return weekday;
    }
}
