using System.Linq.Expressions;

namespace ScheduleService.Application.Contracts;

public interface IScheduleService
{
    void DoIn(Expression<Action> action, DateTimeOffset delay);
    void RecureAction(string id, Expression<Action> action, string cron);
}
