using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Infrastructure.Repositories;

public class WeekdayRepository(IDbContext dbContext) : IWeekdayRepository
{
    private readonly IDbContext _dbContext = dbContext;

    public Task<List<Weekday>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Weekday?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}
