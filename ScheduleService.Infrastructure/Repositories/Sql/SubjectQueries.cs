namespace ScheduleService.Infrastructure.Repositories.Sql;

public static class SubjectQueries
{
    public static readonly string InsertSubject =
        @"
            INSERT INTO subjects (name, abbreviation)
            VALUES (@Name, @Abbreviation)
            RETURNING id;
        ";
    public static readonly string GetSubjectById =
        @"
            SELECT * FROM subjects
            WHERE id = @SubjectId;
        ";
    public static readonly string UpdateSubject =
        @"
            UPDATE subjects
            SET name = @Name,
            abbreviation = @Abbreviation
            WHERE id = @Id;
        ";
    public static readonly string DeleteSubject =
        @"
            DELETE FROM subjects
            WHERE id = @Id
            RETURNING *;
        ";
    public static readonly string GetSubjects =
        @"
            SELECT * FROM subjects
        ";
}
