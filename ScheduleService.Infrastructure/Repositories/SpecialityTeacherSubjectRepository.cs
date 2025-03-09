using System.Data;
using Dapper;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;
using ScheduleService.Infrastructure.Repositories.Sql;

namespace ScheduleService.Infrastructure.Repositories;

public class SpecialityTeacherSubjectRepository(IDbConnection dbConnection)
    : ISpecialityTeacherSubjectRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public Task<SpecialityTeacherSubject?> DeleteAsync(int specialityId, int course, int subgroup)
    {
        throw new NotImplementedException();
    }

    public async Task<List<SpecialityTeacherSubject>> GetAllAsync()
    {
        var result = await _dbConnection.QueryAsync<SpecialityTeacherSubject>(
            SpecialityTeacherSubjectQueries.GetAll
        );

        return [.. result];
    }

    public Task<SpecialityTeacherSubject?> GetByIdAsync(int specialityId, int course, int subgroup)
    {
        throw new NotImplementedException();
    }

    public Task<SpecialityTeacherSubject> InsertAsync(
        SpecialityTeacherSubject specialityTeacherSubject
    )
    {
        throw new NotImplementedException();
    }

    public Task<SpecialityTeacherSubject?> UpdateAsync(
        SpecialityTeacherSubject specialityTeacherSubject
    )
    {
        throw new NotImplementedException();
    }
}
