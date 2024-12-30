using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Teacher;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.GetClassesOnCurrentDateForTeacher;

public class GetClassesOnCurrentDateForTeacherResponse
{
    public Weekday Weekday { get; set; }
    public List<ColorClassesDto<TeacherClassDetailDto>> Classes { get; set; }
}
