using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace ScheduleService.API.Filters;

public class DashBoardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize([NotNull] DashboardContext context)
    {
        return true;
    }
}
