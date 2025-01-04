using Mapster;
using MediatR;
using ScheduleService.Application.Common.Extensions;
using ScheduleService.Application.Common.Specifications.ClassEntity;
using ScheduleService.Application.Contracts.Services;
using ScheduleService.Application.Contracts.UserService.Group;
using ScheduleService.Application.Contracts.UserService.Group.Dto.Responses;
using ScheduleService.Application.Contracts.UserService.Teacher;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student.GetClassesForWeekForStudent;

public class GetClassesForWeekForStudentQueryHandler(
    IClassService classService,
    ITeacherService teacherService,
    IGroupService groupService
) : IRequestHandler<GetClassesForWeekForStudentQuery, GetClassesForWeekForStudentResponse>
{
    private readonly IClassService _classService = classService;
    private readonly IGroupService _groupService = groupService;
    private readonly ITeacherService _teacherService = teacherService;

    public async Task<GetClassesForWeekForStudentResponse> Handle(
        GetClassesForWeekForStudentQuery request,
        CancellationToken cancellationToken
    )
    {
        var classes = await _classService.GetClassesForWeek<StudentClassDetailDto>(
            new GetClassesForWeekForStudentSpecification(request.GroupId)
        );

        foreach (var ColorClasses in classes.Select(x => x.Classes))
        {
            foreach (var colorClass in ColorClasses)
            {
                await colorClass.Classes.LoadTeachers(_teacherService);
            }
        }

        var group = await _groupService.GetGroupById(request.GroupId);

        return new GetClassesForWeekForStudentResponse
        {
            Group = group.Adapt<GroupViewModel>(),
            Classes = classes
        };
    }
}
