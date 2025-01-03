namespace ScheduleService.Infrastructure.Repositories.Sql;

public static class CurrentWeekdayQueries
{
    public static string GetCurrentWeekday =
        @"
            SELECT id, color, interval, updated_at AS UpdatedAt FROM current_weekday
            LIMIT 1;
        ";
    public static string InsertCurrentWeekday =
        @"
            INSERT INTO current_weekday (color, interval)
            VALUES (@Color, @Interval)
            RETURNING id;
        ";
    public static string UpdateCurrentWeekday =
        @"
            UPDATE current_weekday
            SET color = @Color,
            interval = @Interval,
            updated_at = @UpdatedAt
            RETURNING id;
        ";
}
