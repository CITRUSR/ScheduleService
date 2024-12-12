using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.Contracts;

public interface IClassRepository
{
    Task<Class> InsertAsync(Class @class);
    Task<Class?> UpdateAsync(Class @class);
    Task<Class?> DeleteAsync(int id);
    Task<Class?> GetByIdAsync(int id);
    Task<List<Class>> GetAsync();
}
