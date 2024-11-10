using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Infrastructure.Repositories;

public class SubjectRepository(IDbContext dbContext) : ISubjectRepository
{
    private readonly IDbContext _dbContext = dbContext;

    public Task<Subject?> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Subject>> GetAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Subject?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Subject> InsertAsync(Subject subject)
    {
        throw new NotImplementedException();
    }

    public Task<Subject?> UpdateAsync(Subject subject)
    {
        throw new NotImplementedException();
    }
}
