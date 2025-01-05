using ScheduleService.Application.Contracts.UserService.Teacher.dto.responses;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Teacher.GetClassesForWeekForTeacher;

public class GetClassesForWeekForTeacherResponse
{
    public TeacherViewModel Teacher { get; set; }
    public List<
        WeekdayColorClassesDto<ColorClassesDto<TeacherClassDetailDto>, TeacherClassDetailDto>
    > Classes { get; set; }
}
