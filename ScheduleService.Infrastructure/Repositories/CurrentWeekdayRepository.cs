using System.Data;
using Dapper;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;
using ScheduleService.Infrastructure.Repositories.Sql;

namespace ScheduleService.Infrastructure.Repositories;

public class CurrentWeekdayRepository(IDbConnection dbConnection) : ICurrentWeekdayRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task<CurrentWeekday?> GetAsync()
    {
        var currentWeekday = await _dbConnection.QueryFirstOrDefaultAsync<CurrentWeekday>(
            CurrentWeekdayQueries.GetCurrentWeekday
        );

        return currentWeekday;
    }

    public async Task<CurrentWeekday> InsertAsync(CurrentWeekday currentWeekday)
    {
        int id = await _dbConnection.QuerySingleAsync<int>(
            CurrentWeekdayQueries.InsertCurrentWeekday,
            new { currentWeekday.Color, currentWeekday.Interval }
        );

        currentWeekday.Id = id;

        return currentWeekday;
    }

    public async Task<CurrentWeekday?> UpdateAsync(CurrentWeekday currentWeekday)
    {
        var id = await _dbConnection.QueryAsync<int>(
            CurrentWeekdayQueries.UpdateCurrentWeekday,
            new
            {
                currentWeekday.Color,
                currentWeekday.Interval,
                currentWeekday.UpdatedAt,
            }
        );

        if (!id.Any())
        {
            return null;
        }

        currentWeekday.Id = id.First();

        return currentWeekday;
    }
}
