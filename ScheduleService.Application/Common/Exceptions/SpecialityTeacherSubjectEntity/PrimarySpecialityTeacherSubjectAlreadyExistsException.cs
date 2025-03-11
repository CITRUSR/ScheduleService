namespace ScheduleService.Application.Common.Exceptions.SpecialityTeacherSubjectEntity;

public class PrimarySpecialityTeacherSubjectAlreadyExistsException : AlreadyExistsException
{
    public PrimarySpecialityTeacherSubjectAlreadyExistsException(
        int specialityId,
        int course,
        int subgroup
    )
        : base(
            @$"Speciality teacher subject with speciality id:{specialityId},
            course:{course}, subgroup:{subgroup} already exists"
        ) { }
}
