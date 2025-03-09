using System.Data;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Infrastructure.Repositories;

public class SpecialityTeacherSubjectRepository(IDbConnection dbConnection)
    : ISpecialityTeacherSubjectRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public Task<SpecialityTeacherSubject?> DeleteAsync(int specialityId, int course, int subgroup)
    {
        throw new NotImplementedException();
    }

    public Task<List<SpecialityTeacherSubject>> GetAllAsync()
    {
        throw new NotImplementedException();
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
