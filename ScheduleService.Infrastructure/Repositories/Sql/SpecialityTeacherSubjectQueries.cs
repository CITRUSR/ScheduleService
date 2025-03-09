namespace ScheduleService.Infrastructure.Repositories.Sql;

public static class SpecialityTeacherSubjectQueries
{
    private static readonly string AllColumns =
        @"speciality_fk AS SpecialityId, course, subgroup, teacher_fk AS TeacherId, subject_fk AS SubjectId";

    public static readonly string GetAll =
        @$"
        SELECT {AllColumns}
        FROM specialities_teachers_subjects
    ";

    public static readonly string GetById =
        @$"
        SELECT {AllColumns}
        FROM specialities_teachers_subjects
        WHERE speciality_fk = @SpecialityId AND
        course = @Course AND
        subgroup = @Subgroup
    ";
}
