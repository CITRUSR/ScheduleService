namespace ScheduleService.Application.Common.Exceptions.SpecialityTeacherSubjectEntity;

public class SpecialityTeacherSubjectNotFoundException : NotFoundException
{
    public SpecialityTeacherSubjectNotFoundException(int specialityId, int course, int subgroup)
        : base(
            $"speciality teacher subject with speciality id:{specialityId}, course:{course}, subgroup:{subgroup} not found"
        ) { }
}
