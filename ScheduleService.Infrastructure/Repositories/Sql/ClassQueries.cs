namespace ScheduleService.Infrastructure.Repositories.Sql;

public static class ClassQueries
{
    public static readonly string InsertClass =
        @"
            WITH inserted AS (
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
                RETURNING
                    id,
                    group_fk AS GroupId,
                    subject_fk AS SubjectId,
                    weekday_fk AS WeekdayId,
                    color_fk AS ColorId,
                    ends_at AS EndsAt,
                    starts_at AS StartsAt,
                    change_on AS ChangeOn
            ),
            teacher_ids AS(
                SELECT unnest(ARRAY[{0}]) AS teacher_fk
            ),
            room_ids AS(
                SELECT unnest(ARRAY[{1}]) AS room_fk
            ),
            inserted_teachers AS(
                INSERT INTO teachers_classes (class_fk, teacher_fk)
                SELECT inserted.id, teacher_ids.teacher_fk
                FROM inserted, teacher_ids
                RETURNING class_fk, teacher_fk AS teacherId
            ),
            inserted_rooms AS(
                INSERT INTO classes_rooms (class_fk, room_fk)
                SELECT inserted.id, room_ids.room_fk
                FROM inserted, room_ids
                RETURNING class_fk, room_fk
            )

            SELECT
                inserted.id,
                inserted.GroupId,
                inserted.ColorId,
                inserted.EndsAt,
                inserted.StartsAt,
                inserted.ChangeOn,
                subjects.id AS subjectId,
                subjects.name,
                subjects.abbreviation,
                weekdays.id AS weekdayId,
                weekdays.name,
                JSON_AGG(inserted_teachers.teacherId) AS teachers,
                JSON_AGG(rooms) AS rooms
            FROM
                inserted
            JOIN
                subjects ON subjects.id = inserted.SubjectId
            JOIN
                weekdays ON weekdays.id = inserted.WeekdayId
            JOIN
                inserted_teachers ON inserted_teachers.class_fk = inserted.id
            JOIN
                inserted_rooms ON inserted_rooms.class_fk = inserted.id
            JOIN
                rooms ON inserted_rooms.room_fk = rooms.id
            GROUP BY inserted.id,
                     subjects.id,
                     weekdays.id,
                     inserted.GroupId,
                     inserted.ColorId,
                     inserted.EndsAt,
                     inserted.StartsAt,
                     inserted.ChangeOn;
        ";

    public static readonly string UpdateClass =
        @"
            DELETE FROM teachers_classes
            WHERE class_fk = @ClassId AND
            teacher_fk NOT IN (SELECT unnest(ARRAY[{0}]));

            DELETE FROM classes_rooms
            WHERE class_fk = @ClassId AND
            room_fk NOT IN (SELECT unnest(ARRAY[{1}]));

            INSERT INTO teachers_classes (class_fk, teacher_fk)
            SELECT @ClassId, t.teacher_fk
            FROM (SELECT unnest(ARRAY[{0}]) AS teacher_fk) AS t
            WHERE NOT EXISTS (
                SELECT 1
                FROM teachers_classes
                WHERE class_fk = @ClassId AND
                teacher_fk = t.teacher_fk
            );

            INSERT INTO classes_rooms (class_fk, room_fk)
            SELECT @ClassId, r.room_fk
            FROM (
                SELECT unnest(ARRAY[{1}]) as room_fk
            ) AS r
            WHERE NOT EXISTS (
                SELECT 1
                FROM classes_rooms
                WHERE class_fk = @ClassId AND
                room_fk = r.room_fk
            );

            WITH updatedClass AS(
                UPDATE classes
                SET group_fk = @GroupFk,
                    subject_fk = @SubjectFk,
                    weekday_fk = @WeekdayFk,
                    color_fk = @ColorFk,
                    starts_at = @StartsAt,
                    ends_at = @EndsAt,
                    change_on = @ChangeOn
                WHERE id = @ClassId
                RETURNING
                id,
                group_fk AS GroupId,
                subject_fk AS SubjectId,
                weekday_fk AS WeekdayId,
                color_fk AS ColorId,
                ends_at AS EndsAt,
                starts_at AS StartsAt,
                change_on AS ChangeOn
            )

            SELECT
                updatedClass.id,
                updatedClass.GroupId,
                updatedClass.EndsAt,
                updatedClass.StartsAt,
                updatedClass.ChangeOn,
                subjects.id,
                subjects.name,
                subjects.abbreviation,
                weekdays.id,
                weekdays.name,
                colors.id,
                colors.name,
                JSON_AGG(teachers_classes.teacher_fk) AS teachersIds,
                JSON_AGG(rooms) AS rooms
            FROM updatedClass
            JOIN
                subjects ON subjects.id = updatedClass.SubjectId
            JOIN
                weekdays ON weekdays.id = updatedClass.WeekdayId
            JOIN
                teachers_classes ON teachers_classes.class_fk = updatedClass.id
            JOIN
                classes_rooms ON classes_rooms.class_fk = updatedClass.id
            JOIN
                rooms ON rooms.id = classes_rooms.room_fk
            LEFT JOIN
                colors ON colors.id = updatedClass.ColorId
            GROUP BY
                updatedClass.id,
                updatedClass.GroupId,
                updatedClass.EndsAt,
                updatedClass.StartsAt,
                updatedClass.ChangeOn,
                subjects.id,
                subjects.name,
                subjects.abbreviation,
                weekdays.id,
                weekdays.name,
                colors.id,
                colors.name;
                ";
}
