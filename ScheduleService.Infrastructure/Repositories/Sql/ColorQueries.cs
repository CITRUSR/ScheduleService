namespace ScheduleService.Infrastructure.Repositories.Sql;

public static class ColorQueries
{
    public static string InsertColor =
        @"
        INSERT INTO colors (name)
        VALUES (@Name) RETURNING id;
        ";
    public static string GetColorById = 
        @"
            SELECT * FROM colors
            WHERE id = @Id;
        ";
}
