namespace ScheduleService.Infrastructure.Repositories.Sql;

public static class ClassQueries
{
    public static readonly string InsertClass =
        @"
            INSERT INTO classes (
            group_fk,
            subject_fk,
            weekday_fk,
            color_fk,
            starts_at,
            ends_at,
            change_on
            )
            VALUES (
            @GroupFk,
            @SubjectFk,
            @WeekdayFk,
            @ColorFk,
            @StartsAt,
            @EndsAt,
            @ChangeOn
            )
            RETURNING id;
        ";
}
