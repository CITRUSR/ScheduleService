namespace ScheduleService.Infrastructure.Repositories.Sql;

public static class CurrentWeekdayQueries
{
    public static string GetCurrentWeekday =
        @"
            SELECT id, color, interval, updated_at AS UpdatedAt FROM current_weekday
            LIMIT 1;
        ";
}
