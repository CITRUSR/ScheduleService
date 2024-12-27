using ScheduleService.Application.Common.Specifications.ClassEntity;
using ScheduleService.Application.CQRS.ClassEntity;
using ScheduleService.Application.CQRS.ClassEntity.Commands.CreateClass;
using ScheduleService.Application.CQRS.ClassEntity.Commands.UpdateClass;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.Contracts;

public interface IClassRepository
{
    Task<Class> InsertAsync(CreateClassDto dto);
    Task<ClassDependenciesDto> GetClassDependencies(GetClassDependenciesDto dto);
    Task<Class?> UpdateAsync(UpdateClassDto dto);
    Task<Class?> DeleteAsync(int id);
    Task<Class?> GetByIdAsync(int id);
    Task<List<Class>> GetAsync(IClassSpecification specification);
}
