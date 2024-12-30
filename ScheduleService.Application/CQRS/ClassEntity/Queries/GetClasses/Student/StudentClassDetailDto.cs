namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student;

public class StudentClassDetailDto : ClassDetailBase
{
    public List<Guid> TeacherIds { get; set; }
}
