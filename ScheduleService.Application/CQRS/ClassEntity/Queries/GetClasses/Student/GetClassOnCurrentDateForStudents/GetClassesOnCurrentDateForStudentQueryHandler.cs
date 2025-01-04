using Mapster;
using MediatR;
using ScheduleService.Application.Common.Extensions;
using ScheduleService.Application.Common.Specifications.ClassEntity;
using ScheduleService.Application.Contracts.Services;
using ScheduleService.Application.Contracts.UserService.Group;
using ScheduleService.Application.Contracts.UserService.Group.Dto.Responses;
using ScheduleService.Application.Contracts.UserService.Teacher;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student.GetClassOnCurrentDateForStudents;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.GetClassOnCurrentDateForStudents;

public class GetClassesOnCurrentDateForStudentQueryHandler(
    IClassService classService,
    ITeacherService teacherService,
    IGroupService groupService
)
    : IRequestHandler<
        GetClassesOnCurrentDateForStudentQuery,
        GetClassesOnCurrentDateForStudentResponse
    >
{
    private readonly IClassService _classService = classService;
    private readonly IGroupService _groupService = groupService;
    private readonly ITeacherService _teacherService = teacherService;

    public async Task<GetClassesOnCurrentDateForStudentResponse> Handle(
        GetClassesOnCurrentDateForStudentQuery request,
        CancellationToken cancellationToken
    )
    {
        var CurrentWeekdayOrder = DateTime.Now.GetCurrentWeekdayOrder();

        var (classes, weekday) = await _classService.GetClassesForDay<StudentClassDetailDto>(
            new GetClassesOnCurrentDateForStudentSpecification(
                request.GroupId,
                CurrentWeekdayOrder
            ),
            CurrentWeekdayOrder
        );

        foreach (var @class in classes)
        {
            await @class.Classes.LoadTeachers(_teacherService);
        }

        var group = await _groupService.GetGroupById(request.GroupId);

        var result = new GetClassesOnCurrentDateForStudentResponse
        {
            Classes = classes,
            Group = group.Adapt<GroupViewModel>(),
            Weekday = weekday
        };

        return result;
    }
}
