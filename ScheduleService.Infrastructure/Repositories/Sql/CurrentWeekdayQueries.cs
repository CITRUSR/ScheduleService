namespace ScheduleService.Infrastructure.Repositories.Sql;

public static class CurrentWeekdayQueries
{
    public static readonly string GetCurrentWeekday =
        @"
            SELECT id, color, interval, updated_at AS UpdatedAt FROM current_weekday
            LIMIT 1;
        ";
    public static readonly string InsertCurrentWeekday =
        @"
            INSERT INTO current_weekday (color, interval)
            VALUES (@Color, @Interval)
            RETURNING id;
        ";
    public static readonly string UpdateCurrentWeekday =
        @"
            UPDATE current_weekday
            SET color = @Color,
            interval = @Interval,
            updated_at = @UpdatedAt
            RETURNING id;
        ";
}
