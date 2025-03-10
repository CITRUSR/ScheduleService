namespace ScheduleService.Application.Common.Exceptions.SpecialityTeacherSubjectEntity;

public class TeacherSubjectCombinationAlreadyExistsException : AlreadyExistsException
{
    public TeacherSubjectCombinationAlreadyExistsException(Guid teacherId, int subjectId)
        : base(
            @$"The teacher subject combination with teacher id:{teacherId},
            subject id:{subjectId} already exists"
        ) { }
}
