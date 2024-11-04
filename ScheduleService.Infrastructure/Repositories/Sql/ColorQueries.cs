namespace ScheduleService.Infrastructure.Repositories.Sql;

public static class ColorQueries
{
    public static string InsertColor =
        @"
        INSERT INTO colors (name)
        VALUES (@Name) RETURNING id;
        ";
}
