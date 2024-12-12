using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Infrastructure.Repositories;

public class ClassRepository(IDbContext dbContext) : IClassRepository
{
    private readonly IDbContext _dbContext = dbContext;

    public Task<Class?> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Class>> GetAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Class?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Class> InsertAsync(Class @class)
    {
        throw new NotImplementedException();
    }

    public Task<Class?> UpdateAsync(Class @class)
    {
        throw new NotImplementedException();
    }
}
