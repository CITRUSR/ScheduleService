namespace ScheduleService.Infrastructure.Repositories.Sql;

public class RoomQueries
{
    public static readonly string InsertRoom =
        @"
            INSERT INTO rooms (name, full_name)
            VALUES (@Name, @FullName)
            RETURNING id;
        ";
    public static readonly string GetRoomById =
        @"
            SELECT id, name, full_name AS FullName FROM rooms
            WHERE id = @Id;
        ";
    public static readonly string UpdateRoom =
        @"
            UPDATE rooms
            SET name = @Name,
            full_name = @FullName
            WHERE id = @Id;
        ";
    public static readonly string DeleteRoom =
        @"
            DELETE FROM rooms
            WHERE id = @Id
            RETURNING id, name, full_name AS FullName;
        ";
    public static readonly string GetRooms =
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

    public static readonly string GetRoomsById =
        @"
            SELECT * FROM rooms
            WHERE id IN ({0})
        ";
}
