using Dapper;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;
using ScheduleService.Infrastructure.Repositories.Sql;

namespace ScheduleService.Infrastructure.Repositories;

public class CurrentWeekdayRepository(IDbContext dbContext) : ICurrentWeekdayRepository
{
    private readonly IDbContext _dbContext = dbContext;

    public async Task<CurrentWeekday?> GetAsync()
    {
        using var connection = _dbContext.CreateConnection();

        var currentWeekday = await connection.QueryFirstOrDefaultAsync<CurrentWeekday>(
            CurrentWeekdayQueries.GetCurrentWeekday
        );

        return currentWeekday;
    }

    public async Task<CurrentWeekday> InsertAsync(CurrentWeekday currentWeekday)
    {
        using var connection = _dbContext.CreateConnection();

        int id = await connection.QuerySingleAsync<int>(
            CurrentWeekdayQueries.InsertCurrentWeekday,
            new { currentWeekday.Color, currentWeekday.Interval }
        );

        currentWeekday.Id = id;

        return currentWeekday;
    }

    public async Task<CurrentWeekday?> UpdateAsync(CurrentWeekday currentWeekday)
    {
        using var connection = _dbContext.CreateConnection();

        var id = await connection.QueryAsync<int>(
            CurrentWeekdayQueries.UpdateCurrentWeekday,
            new
            {
                currentWeekday.Color,
                currentWeekday.Interval,
                currentWeekday.UpdatedAt
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
