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

    public Task<CurrentWeekday> InsertAsync(CurrentWeekday currentWeekday)
    {
        throw new NotImplementedException();
    }

    public Task<CurrentWeekday?> UpdateAsync(CurrentWeekday currentWeekday) { }
}
