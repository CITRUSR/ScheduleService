using ScheduleService.Application.Common.Extensions;

namespace ScheduleService.Application.Common.Specifications.ClassEntity;

public abstract class GetClassesForWeekSpecificationBase : IClassSpecification
{
    public DateTime LeftChangeDateLimiter =>
        DateTime.Now.AddDays(-DateTime.Now.GetCurrentWeekdayOrder());

    public DateTime RightChangeDateLimiter => LeftChangeDateLimiter.AddDays(5);

    public abstract string WhereClause { get; }
}
