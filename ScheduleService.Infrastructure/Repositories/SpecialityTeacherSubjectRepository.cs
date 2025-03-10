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

    public async Task<SpecialityTeacherSubject?> DeleteAsync(
        int specialityId,
        int course,
        int subgroup
    )
    {
        var parameters = new DynamicParameters();

        parameters.Add("@SpecialityId", specialityId);
        parameters.Add("@Course", course);
        parameters.Add("@Subgroup", subgroup);

        var res = await _dbConnection.QuerySingleOrDefaultAsync<SpecialityTeacherSubject>(
            SpecialityTeacherSubjectQueries.Delete,
            parameters
        );

        return res;
    }

    public async Task<List<SpecialityTeacherSubject>> GetAllAsync()
    {
        var result = await _dbConnection.QueryAsync<SpecialityTeacherSubject>(
            SpecialityTeacherSubjectQueries.GetAll
        );

        return [.. result];
    }

    public async Task<SpecialityTeacherSubject?> GetByIdAsync(
        int specialityId,
        int course,
        int subgroup
    )
    {
        var parameters = new DynamicParameters();

        parameters.Add("@SpecialityId", specialityId);
        parameters.Add("@Course", course);
        parameters.Add("@Subgroup", subgroup);

        var entity = await _dbConnection.QuerySingleOrDefaultAsync<SpecialityTeacherSubject>(
            SpecialityTeacherSubjectQueries.GetById,
            parameters
        );

        return entity;
    }

    public async Task<SpecialityTeacherSubject> InsertAsync(
        SpecialityTeacherSubject specialityTeacherSubject
    )
    {
        var parameters = new DynamicParameters();
        parameters.Add("@SpecialityId", specialityTeacherSubject.SpecialityId);
        parameters.Add("@Course", specialityTeacherSubject.Course);
        parameters.Add("@Subgroup", specialityTeacherSubject.SubGroup);
        parameters.Add("@TeacherId", specialityTeacherSubject.TeacherId);
        parameters.Add("@SubjectId", specialityTeacherSubject.SubjectId);

        await _dbConnection.ExecuteAsync(SpecialityTeacherSubjectQueries.Insert, parameters);

        return specialityTeacherSubject;
    }

    public Task<SpecialityTeacherSubject?> UpdateAsync(
        SpecialityTeacherSubject specialityTeacherSubject
    )
    {
        throw new NotImplementedException();
    }
}
