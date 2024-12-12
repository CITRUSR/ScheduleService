using ScheduleService.Application.Contracts.UserService.Teacher.dto.responses;

namespace ScheduleService.Application.Contracts.UserService.Teacher;

public interface ITeacherService
{
    Task<TeacherDto> GetTeacherById(Guid id);
}
