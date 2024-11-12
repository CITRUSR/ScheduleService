namespace ScheduleService.Infrastructure.Repositories.Sql;

public static class WeekdayQueries
{
    public static string GetWeekdayById =
        @"
            SELECT * FROM weekdays
            WHERE id = @Id;
        ";
    public static string GetAllWeekdays =
        @"
            SELECT * FROM weekdays
        ";
}
