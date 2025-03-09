using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.Contracts;

public interface ISpecialityTeacherSubjectRepository
{
    Task<List<SpecialityTeacherSubject>> GetAllAsync();
    Task<SpecialityTeacherSubject?> GetByIdAsync(int specialityId, int course, int subgroup);
    Task<SpecialityTeacherSubject> InsertAsync(SpecialityTeacherSubject specialityTeacherSubject);
    Task<SpecialityTeacherSubject?> UpdateAsync(SpecialityTeacherSubject specialityTeacherSubject);
    Task<SpecialityTeacherSubject?> DeleteAsync(int specialityId, int course, int subgroup);
}
