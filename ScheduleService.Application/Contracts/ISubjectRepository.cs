using ScheduleService.Application.Common.Models;
using ScheduleService.Application.CQRS.SubjectEntity.Queries.GetSubjects;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.Contracts;

public interface ISubjectRepository
{
    Task<List<Subject>> GetAsync(SubjectFilter filter, PaginationParameters paginationParameters);
    Task<Subject?> GetByIdAsync(int id);
    Task<Subject> InsertAsync(Subject subject);
    Task<Subject?> UpdateAsync(Subject subject);
    Task<Subject?> DeleteAsync(int id);
}
