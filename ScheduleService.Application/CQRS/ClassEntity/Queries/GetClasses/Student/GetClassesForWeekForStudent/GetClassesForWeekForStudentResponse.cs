namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student.GetClassesForWeekForStudent;

public class GetClassesForWeekForStudentResponse
{
    public int GroupId { get; set; }
    public List<
        WeekdayColorClassesDto<ColorClassesDto<StudentClassDetailDto>, StudentClassDetailDto>
    > Classes { get; set; }
}
