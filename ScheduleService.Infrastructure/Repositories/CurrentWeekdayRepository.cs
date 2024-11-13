using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Infrastructure.Repositories;

public class CurrentWeekdayRepository(IDbContext dbContext) : ICurrentWeekdayRepository
{
    private readonly IDbContext _dbContext = dbContext;

    public Task<CurrentWeekday?> GetAsync()
    {
        throw new NotImplementedException();
    }

    public Task<CurrentWeekday> InsertAsync(CurrentWeekday currentWeekday)
    {
        throw new NotImplementedException();
    }

    public Task<CurrentWeekday?> UpdateAsync(CurrentWeekday currentWeekday) { }
}
