namespace ScheduleService.Application.Common.Specifications.ClassEntity;

public class GetClassesForWeekForStudentSpecification : IClassSpecification
{
    public DateTime LeftChangeDateLimiter { get; private set; }

    public DateTime RightChangeDateLimiter { get; private set; }
    private readonly int GroupId;

    public GetClassesForWeekForStudentSpecification(
        DateTime leftBorder,
        DateTime rightBorder,
        int groupId
    )
    {
        LeftChangeDateLimiter = leftBorder;
        RightChangeDateLimiter = rightBorder;
        GroupId = groupId;
    }

    public string WhereClause => $@"classes.group_fk = {GroupId} AND irrelevant_since IS NULL";
}
