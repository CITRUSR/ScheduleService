using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.Contracts;

public interface IColorRepository
{
    Task<List<Color>> GetAllAsync();
    Task<Color?> GetByIdAsync(int id);
    Task<Color> InsertAsync(Color color);
    Task<Color> UpdateAsync(Color color);
    Task DeleteAsync(int id);
}
