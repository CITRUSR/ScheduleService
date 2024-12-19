using ScheduleService.Application.CQRS.ClassEntity.Commands.CreateClass;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.Contracts;

public interface IClassRepository
{
    Task<Class> InsertAsync(CreateClassDto dto);
    Task<ClassCreationDto> GetEntitiesForInsertClassAsync(CreateClassDto dto);
    Task<Class?> UpdateAsync(Class @class);
    Task<Class?> DeleteAsync(int id);
    Task<Class?> GetByIdAsync(int id);
    Task<List<Class>> GetAsync();
}
