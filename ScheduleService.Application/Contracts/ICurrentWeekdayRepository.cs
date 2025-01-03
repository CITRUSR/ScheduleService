using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.Contracts;

public interface ICurrentWeekdayRepository
{
    Task<CurrentWeekday> InsertAsync(CurrentWeekday currentWeekday);
    Task<CurrentWeekday?> UpdateAsync(CurrentWeekday currentWeekday);
    Task<CurrentWeekday?> GetAsync();
}
