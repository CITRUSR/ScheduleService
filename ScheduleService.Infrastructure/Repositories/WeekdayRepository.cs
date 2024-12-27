using Dapper;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;
using ScheduleService.Infrastructure.Repositories.Sql;

namespace ScheduleService.Infrastructure.Repositories;

public class WeekdayRepository(IDbContext dbContext) : IWeekdayRepository
{
    private readonly IDbContext _dbContext = dbContext;

    public async Task<List<Weekday>> GetAllAsync()
    {
        using var connection = _dbContext.CreateConnection();

        var weekdays = await connection.QueryAsync<Weekday>(WeekdayQueries.GetAllWeekdays);

        return [.. weekdays];
    }

    public async Task<Weekday?> GetByIdAsync(int id)
    {
        using var connection = _dbContext.CreateConnection();

        var parameters = new DynamicParameters();

        parameters.Add("WeekdayId", id);

        var weekday = await connection.QueryFirstOrDefaultAsync<Weekday>(
            WeekdayQueries.GetWeekdayById,
            parameters
        );

        return weekday;
    }
}
