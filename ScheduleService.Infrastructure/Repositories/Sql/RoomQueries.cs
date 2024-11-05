namespace ScheduleService.Infrastructure.Repositories.Sql;

public class RoomQueries
{
    public static string InsertRoom =
        @"
            INSERT INTO rooms (name, full_name)
            VALUES (@Name, @FullName)
            RETURNING id;
        ";
    public static string GetRoomById =
        @"
            SELECT name, full_name AS FullName FROM rooms
            WHERE id = @Id;
        ";
    public static string UpdateRoom =
        @"
            UPDATE rooms
            SET name = @Name,
            full_name = @FullName
            WHERE id = @Id;
        ";
}
