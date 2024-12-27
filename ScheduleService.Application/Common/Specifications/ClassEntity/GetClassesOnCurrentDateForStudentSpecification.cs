namespace ScheduleService.Application.Common.Specifications.ClassEntity;

public class GetClassesOnCurrentDateForStudentSpecification(int groupId, int weekdayId)
    : IClassSpecification
{
    public DateTime LeftChangeDateLimiter => DateTime.Now;
    public DateTime RightChangeDateLimiter => DateTime.Now;
    private readonly int GroupId = groupId;
    private readonly int WeekdayId = weekdayId;

    public string WhereClause
    {
        get
        {
            return $@"classes.weekday_fk = {WeekdayId} AND
                classes.group_fk = {GroupId} AND
                irrelevant_since IS NULL";
        }
    }
}
