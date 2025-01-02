namespace ScheduleService.Application.Common.Specifications.ClassEntity;

public class GetClassesOnCurrentDateForTeacherSpecification(Guid teacherId, int weekdayId)
    : IClassSpecification
{
    public DateTime LeftChangeDateLimiter => DateTime.Now;

    public DateTime RightChangeDateLimiter => DateTime.Now;

    private readonly Guid TeacherId = teacherId;
    private readonly int WeekdayId = weekdayId;

    public string WhereClause =>
        @$"teachers_classes.teacher_fk = '{TeacherId}' AND
           classes.weekday_fk = {WeekdayId} ";
}
