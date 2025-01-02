namespace ScheduleService.Application.Common.Specifications.ClassEntity;

public class GetClassesForWeekForTeacherSpecification(Guid teacherId)
    : GetClassesForWeekSpecificationBase
{
    private readonly Guid _teacherId = teacherId;

    public override string WhereClause =>
        @$"teachers_classes.teacher_fk = '{_teacherId}' AND irrelevant_since IS NULL";
}
