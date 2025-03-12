namespace ScheduleService.Infrastructure.Repositories.Sql;

public static class WeekdayQueries
{
    public static readonly string GetWeekdayById =
        @"
            SELECT * FROM weekdays
            WHERE id = @WeekdayId;
        ";
    public static readonly string GetAllWeekdays =
        @"
            SELECT * FROM weekdays
        ";
}
