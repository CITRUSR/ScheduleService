namespace ScheduleService.Application.Common.Specifications.ClassEntity;

public class GetClassesForWeekForStudentSpecification(int groupId)
    : GetClassesForWeekSpecificationBase
{
    private readonly int GroupId = groupId;

    public override string WhereClause =>
        $@"classes.group_fk = {GroupId} AND irrelevant_since IS NULL";
}
