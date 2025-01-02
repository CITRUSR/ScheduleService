using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.GetClassOnCurrentDateForStudents;

public class GetClassesOnCurrentDateForStudentResponse
{
    public int GroupId { get; set; }
    public Weekday Weekday { get; set; }
    public List<ColorClassesDto<StudentClassDetailDto>> Classes { get; set; }
}
