namespace ScheduleService.Infrastructure.Repositories.Sql;

public class RoomQueries
{
    public static string InsertRoom =
        @"
            INSERT INTO rooms (name, full_name)
            VALUES (@Name, @FullName)
            RETURNING id;
        ";
}
