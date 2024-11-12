using Dapper;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;
using ScheduleService.Infrastructure.Repositories.Sql;

namespace ScheduleService.Infrastructure.Repositories;

public class WeekdayRepository(IDbContext dbContext) : IWeekdayRepository
{
    private readonly IDbContext _dbContext = dbContext;

    public Task<List<Weekday>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Weekday?> GetByIdAsync(int id)
    {
        using var connection = _dbContext.CreateConnection();

        var weekday = await connection.QueryFirstOrDefaultAsync<Weekday>(
            WeekdayQueries.GetWeekdayById,
            new { id }
        );

        return weekday;
    }
}
