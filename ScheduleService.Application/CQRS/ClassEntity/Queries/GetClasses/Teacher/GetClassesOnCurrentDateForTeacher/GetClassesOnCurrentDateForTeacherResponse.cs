using ScheduleService.Application.Contracts.UserService.Teacher.dto.responses;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Teacher;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.GetClassesOnCurrentDateForTeacher;

public class GetClassesOnCurrentDateForTeacherResponse
{
    public TeacherViewModel Teacher { get; set; }
    public Weekday Weekday { get; set; }
    public List<ColorClassesDto<TeacherClassDetailDto>> Classes { get; set; }
}
