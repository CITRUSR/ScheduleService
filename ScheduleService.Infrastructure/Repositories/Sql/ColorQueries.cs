namespace ScheduleService.Infrastructure.Repositories.Sql;

public static class ColorQueries
{
    public static readonly string InsertColor =
        @"
        INSERT INTO colors (name)
        VALUES (@Name) RETURNING id;
        ";
    public static readonly string GetColorById =
        @"
            SELECT * FROM colors
            WHERE id = @ColorId;
        ";
    public static readonly string UpdateColor =
        @"
            UPDATE colors
            SET name = @Name
            WHERE id = @Id;
        ";
    public static readonly string DeleteColor =
        @"
            DELETE FROM colors
            WHERE id = @Id
            RETURNING id, name;
        ";
    public static readonly string GetAllColors =
        @"
            SELECT * FROM colors;
        ";
}
