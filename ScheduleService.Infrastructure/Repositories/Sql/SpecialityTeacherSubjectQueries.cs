namespace ScheduleService.Infrastructure.Repositories.Sql;

public static class SpecialityTeacherSubjectQueries
{
    public static readonly string GetAll =
        @$"
        SELECT speciality_fk AS SpecialityId, course, subgroup, teacher_fk AS TeacherId, subject_fk AS SubjectId
        FROM specialities_teachers_subjects
    ";
}
