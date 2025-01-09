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
                inserted.EndsAt,
                inserted.StartsAt,
                inserted.ChangeOn,
                inserted.ColorId AS id,
                colors.name,
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
            LEFT JOIN
                colors ON colors.id = ColorId
            GROUP BY inserted.id,
                     subjects.id,
                     weekdays.id,
                     inserted.GroupId,
                     inserted.ColorId,
                     colors.name,
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
    public static readonly string GetClassById =
        @"
            SELECT
                classes.id,
                classes.group_fk AS GroupId,
                classes.ends_at AS EndsAt,
                classes.starts_at AS StartsAt,
                classes.change_on AS ChangeOn,
                classes.irrelevant_since AS IrrelevantSince,
                subjects.id,
                subjects.name,
                subjects.abbreviation,
                weekdays.id,
                weekdays.name,
                colors.id,
                colors.name,
                JSON_AGG(teachers_classes.teacher_fk) AS TeachersIds,
                JSON_AGG(rooms) AS rooms
            FROM classes
            JOIN
                subjects ON subjects.id = classes.subject_fk
            JOIN
                weekdays ON weekdays.id = classes.weekday_fk
            JOIN
                teachers_classes ON teachers_classes.class_fk = @ClassId
            JOIN 
                classes_rooms ON classes_rooms.class_fk = @ClassId
            JOIN
                rooms ON rooms.id = classes_rooms.room_fk
            LEFT JOIN
                colors ON colors.id = classes.color_fk
            WHERE classes.id = @ClassId
            GROUP BY
                classes.id,
                classes.group_fk,
                classes.ends_at,
                classes.starts_at,
                classes.change_on,
                classes.irrelevant_since,
                subjects.id,
                subjects.name,
                subjects.abbreviation,
                weekdays.id,
                weekdays.name,
                colors.id,
                colors.name;
        ";
    public static readonly string DeleteClass =
        GetClassById
        + @"
            DELETE FROM classes
            WHERE id = @ClassId;
        ";

    //join all related tables with a join,
    //even if they are not needed in the query for a flexible system with a dynamic where
    public static readonly string GetClasses =
        @"
             WITH changes AS(
                SELECT
                    classes.id,
                    classes.starts_at AS StartsAt,
                    classes.ends_at AS EndsAt,
                    classes.change_on AS ChangeOn,
                    classes.irrelevant_since AS IrrelevantSince,
                    classes.group_fk AS GroupId,
                    classes.weekday_fk AS WeekdayId,
                    weekdays.name AS WeekdayName,
                    subjects.id AS SubjectId,
                    subjects.name AS SubjectName,
                    subjects.abbreviation AS SubjectAbbreviation,
                    colors.id AS ColorId,
                    colors.name AS ColorName
                FROM classes
                JOIN
                    subjects on subjects.id = classes.subject_fk
                LEFT JOIN
                    colors ON colors.id = classes.color_fk
                JOIN
                    teachers_classes ON teachers_classes.class_fk = classes.id
                JOIN
                    classes_rooms ON classes_rooms.class_fk = classes.id
                JOIN
                    rooms ON classes_rooms.room_fk = rooms.id
                JOIN
                    weekdays ON weekdays.id = classes.weekday_fk
                WHERE classes.change_on <= @RightChangeDateLimiter AND
                classes.change_on >= @LeftChangeDateLimiter AND
                {0}
            )

            SELECT
                classes.id,
                classes.starts_at AS StartsAt,
                classes.ends_at AS EndsAt,
                classes.change_on AS ChangeOn,
                classes.group_fk AS GroupId,
                classes.weekday_fk AS id,
                weekdays.name,
                subjects.id,
                subjects.name,
                subjects.abbreviation,
                colors.id,
                colors.name,
                JSON_AGG(teachers_classes.teacher_fk) AS teacherIds,
                JSON_AGG(rooms) AS rooms
            FROM
                classes
            JOIN
                subjects ON classes.subject_fk = subjects.id
            LEFT JOIN
               changes on classes.id = changes.id
            JOIN
                teachers_classes ON teachers_classes.class_fk = classes.id
            JOIN
                classes_rooms ON classes_rooms.class_fk = classes.id
            JOIN
                rooms ON classes_rooms.room_fk = rooms.id
            LEFT JOIN
                colors on classes.color_fk = colors.id
            JOIN
                weekdays ON weekdays.id = classes.weekday_fk
            WHERE changes.id IS NULL AND
             {0}
            GROUP BY
                classes.id,
                classes.starts_at,
                classes.ends_at,
                classes.change_on,
                classes.group_fk,
                classes.weekday_fk,
                weekdays.name,
                subjects.id,
                subjects.name,
                subjects.abbreviation,
                colors.id,
                colors.name

            UNION ALL

            SELECT
                changes.id,
                changes.StartsAt,
                changes.EndsAt,
                changes.ChangeOn,
                changes.GroupId,
                changes.WeekdayId AS id,
                changes.WeekdayName AS name,
                changes.SubjectId AS id,
                changes.SubjectName AS name,
                changes.SubjectAbbreviation AS abbreviation,
                changes.ColorId AS id,
                changes.ColorName AS name,
                JSON_AGG(teachers_classes.teacher_fk) AS teacherIds,
                JSON_AGG(rooms) AS rooms
            FROM
                changes
            JOIN
                teachers_classes ON teachers_classes.class_fk = changes.id
            JOIN
                classes_rooms ON classes_rooms.class_fk = changes.id
            JOIN
                rooms ON classes_rooms.room_fk = rooms.id
            JOIN
                weekdays ON weekdays.id = changes.WeekdayId
            GROUP BY
                changes.id,
                changes.StartsAt,
                changes.EndsAt,
                changes.ChangeOn,
                changes.GroupId,
                changes.WeekdayId,
                changes.WeekdayName,
                changes.SubjectId,
                changes.SubjectName,
                changes.SubjectAbbreviation,
                changes.ColorId,
                changes.ColorName
            ORDER BY StartsAt
        ";
}
