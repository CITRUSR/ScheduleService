using ScheduleService.Application.Contracts.UserService.Teacher.dto.responses;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student;

public class StudentClassDetailDto : ClassDetailBase
{
    public List<Guid> TeacherIds { get; set; } = [];
    public List<TeacherViewModel> Teachers { get; set; } = [];
}
