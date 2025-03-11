using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.Contracts.Services;

public interface ISpecialityTeacherSubjectRelatedDataChecker
{
    Task Check(SpecialityTeacherSubject specialityTeacherSubject);
}
