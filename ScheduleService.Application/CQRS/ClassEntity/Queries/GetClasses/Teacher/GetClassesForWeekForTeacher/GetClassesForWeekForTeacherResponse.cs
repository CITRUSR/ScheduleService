namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Teacher.GetClassesForWeekForTeacher;

public class GetClassesForWeekForTeacherResponse
{
    public Guid TeacherId { get; set; }
    public List<
        WeekdayColorClassesDto<ColorClassesDto<TeacherClassDetailDto>, TeacherClassDetailDto>
    > Classes { get; set; }
}
