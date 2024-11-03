using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Infrastructure.Repositories;

public class ColorRepository(IDbContext dbContext) : IColorRepository
{
    private readonly IDbContext _dbContext = dbContext;

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Color>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Color> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Color> InsertAsync(Color color)
    {
        throw new NotImplementedException();
    }

    public Task<Color> UpdateAsync(Color color)
    {
        throw new NotImplementedException();
    }
}
