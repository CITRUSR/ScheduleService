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
            SELECT id, name, full_name AS FullName FROM rooms
            WHERE id = @Id;
        ";
    public static string UpdateRoom =
        @"
            UPDATE rooms
            SET name = @Name,
            full_name = @FullName
            WHERE id = @Id;
        ";
    public static string DeleteRoom =
        @"
            DELETE FROM rooms
            WHERE id = @Id
            RETURNING id, name, full_name AS FullName;
        ";
    public static string GetRooms =
        @"
            WITH filtered_rooms AS(
                SELECT id, name, full_name AS FullName FROM rooms
                {0}
            )
            SELECT id, name, FullName, COUNT(*) OVER() AS totalCount
            FROM filtered_rooms
            ORDER BY {1} {2}
            LIMIT @Limit
            OFFSET @Offset
        ";

    public static string GetRoomsById =
        @"
            SELECT * FROM rooms
            WHERE id IN ({0})
        ";
}
