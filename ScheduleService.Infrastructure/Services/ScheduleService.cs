using System.Linq.Expressions;
using Hangfire;
using ScheduleService.Application.Contracts;

namespace ScheduleService.Infrastructure.Services;

public class ScheduleService : IScheduleService
{
    public void DoIn(Expression<Action> action, DateTimeOffset delay)
    {
        BackgroundJob.Schedule(action, delay);
    }

    public void RecureAction(string id, Expression<Action> action, string cron)
    {
        RecurringJob.AddOrUpdate(id, action, cron);
    }
}
