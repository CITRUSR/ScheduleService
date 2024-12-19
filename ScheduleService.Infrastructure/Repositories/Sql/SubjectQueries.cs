namespace ScheduleService.Infrastructure.Repositories.Sql;

public static class SubjectQueries
{
    public static string InsertSubject =
        @"
            INSERT INTO subjects (name, abbreviation)
            VALUES (@Name, @Abbreviation)
            RETURNING id;
        ";
    public static string GetSubjectById =
        @"
            SELECT * FROM subjects
            WHERE id = @SubjectId;
        ";
    public static string UpdateSubject =
        @"
            UPDATE subjects
            SET name = @Name,
            abbreviation = @Abbreviation
            WHERE id = @Id;
        ";
    public static string DeleteSubject =
        @"
            DELETE FROM subjects
            WHERE id = @Id
            RETURNING *;
        ";
    public static string GetSubjects =
        @"
            SELECT * FROM subjects
        ";
}
