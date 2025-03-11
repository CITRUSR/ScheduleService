using MediatR;
using ScheduleService.Application.Contracts.Services;
using ScheduleService.Application.Contracts.UserService.Speciality;
using ScheduleService.Application.Contracts.UserService.Teacher;
using ScheduleService.Application.CQRS.SubjectEntity.Queries.GetSubjectById;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.Common.Services;

public class SpecialityTeacherSubjectRelatedDataChecker(
    IMediator mediator,
    ITeacherService teacherService,
    ISpecialityService specialityService
) : ISpecialityTeacherSubjectRelatedDataChecker
{
    private readonly IMediator _mediator = mediator;
    private readonly ITeacherService _teacherService = teacherService;
    private readonly ISpecialityService _specialityService = specialityService;

    public async Task Check(SpecialityTeacherSubject specialityTeacherSubject)
    {
        var subjectQuery = new GetSubjectByIdQuery(specialityTeacherSubject.SubjectId);

        await _mediator.Send(subjectQuery);

        List<Task> checkTasks =
        [
            _teacherService.GetTeacherById(specialityTeacherSubject.TeacherId),
            _specialityService.GetSpecialityById(specialityTeacherSubject.SpecialityId),
        ];

        await Task.WhenAll(checkTasks);
    }
}
