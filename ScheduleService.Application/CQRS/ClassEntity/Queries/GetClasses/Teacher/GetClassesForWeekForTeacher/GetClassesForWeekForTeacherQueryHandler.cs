using Mapster;
using MediatR;
using ScheduleService.Application.Common.Extensions;
using ScheduleService.Application.Common.Specifications.ClassEntity;
using ScheduleService.Application.Contracts.Services;
using ScheduleService.Application.Contracts.UserService.Group;
using ScheduleService.Application.Contracts.UserService.Teacher;
using ScheduleService.Application.Contracts.UserService.Teacher.dto.responses;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Teacher.GetClassesForWeekForTeacher;

public class GetClassesForWeekForTeacherQueryHandler(
    IClassService classService,
    IGroupService groupService,
    ITeacherService teacherService
) : IRequestHandler<GetClassesForWeekForTeacherQuery, GetClassesForWeekForTeacherResponse>
{
    private readonly IClassService _classService = classService;
    private readonly IGroupService _groupService = groupService;
    private readonly ITeacherService _teacherService = teacherService;

    public async Task<GetClassesForWeekForTeacherResponse> Handle(
        GetClassesForWeekForTeacherQuery request,
        CancellationToken cancellationToken
    )
    {
        var classes = await _classService.GetClassesForWeek<TeacherClassDetailDto>(
            new GetClassesForWeekForTeacherSpecification(request.TeacherId)
        );

        foreach (
            var classDetails in classes.Select(x => x.Classes.SelectMany(c => c.Classes).ToList())
        )
        {
            await classDetails.LoadGroups(_groupService);
        }

        var teacher = await _teacherService.GetTeacherById(request.TeacherId);

        return new GetClassesForWeekForTeacherResponse
        {
            Classes = classes,
            Teacher = teacher.Adapt<TeacherViewModel>()
        };
    }
}
