using Mapster;
using MediatR;
using ScheduleService.Application.Common.Extensions;
using ScheduleService.Application.Common.Specifications.ClassEntity;
using ScheduleService.Application.Contracts.Services;
using ScheduleService.Application.Contracts.UserService.Group;
using ScheduleService.Application.Contracts.UserService.Teacher;
using ScheduleService.Application.Contracts.UserService.Teacher.dto.responses;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Teacher;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.GetClassesOnCurrentDateForTeacher;

public class GetClassesOnCurrentDateForTeacherQueryHandler(
    IClassService classService,
    IGroupService groupService,
    ITeacherService teacherService
)
    : IRequestHandler<
        GetClassesOnCurrentDateForTeacherQuery,
        GetClassesOnCurrentDateForTeacherResponse
    >
{
    private readonly IClassService _classService = classService;
    private readonly IGroupService _groupService = groupService;
    private readonly ITeacherService _teacherService = teacherService;

    public async Task<GetClassesOnCurrentDateForTeacherResponse> Handle(
        GetClassesOnCurrentDateForTeacherQuery request,
        CancellationToken cancellationToken
    )
    {
        int weekdayId = DateTime.Now.GetCurrentWeekdayOrder();

        var (classes, weekday) = await _classService.GetClassesForDay<TeacherClassDetailDto>(
            new GetClassesOnCurrentDateForTeacherSpecification(request.TeacherId, weekdayId),
            weekdayId
        );

        foreach (var classDetails in classes.Select(x => x.Classes))
        {
            await classDetails.LoadGroups(_groupService);
        }

        var teacher = await _teacherService.GetTeacherById(request.TeacherId);

        return new GetClassesOnCurrentDateForTeacherResponse
        {
            Classes = classes,
            Weekday = weekday,
            Teacher = teacher.Adapt<TeacherViewModel>()
        };
    }
}
