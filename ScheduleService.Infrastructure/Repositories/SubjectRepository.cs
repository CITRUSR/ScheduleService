using Dapper;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;
using ScheduleService.Infrastructure.Repositories.Sql;

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

    public async Task<Subject?> GetByIdAsync(int id)
    {
        using var connection = _dbContext.CreateConnection();

        var subject = await connection.QueryFirstOrDefaultAsync<Subject>(
            SubjectQueries.GetSubjectById,
            new { id }
        );

        return subject;
    }

    public async Task<Subject> InsertAsync(Subject subject)
    {
        using var connection = _dbContext.CreateConnection();

        var subjectId = await connection.QuerySingleAsync<int>(
            SubjectQueries.InsertSubject,
            new { subject.Name, subject.Abbreviation }
        );

        subject.Id = subjectId;

        return subject;
    }

    public Task<Subject?> UpdateAsync(Subject subject)
    {
        throw new NotImplementedException();
    }
}
