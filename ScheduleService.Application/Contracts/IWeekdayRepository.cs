using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.Contracts;

public interface IWeekdayRepository
{
    Task<List<Weekday>> GetAllAsync();
    Task<Weekday?> GetByIdAsync(int id);
}
