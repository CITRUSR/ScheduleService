using MediatR;
using ScheduleService.Application.Common.Extensions;
using ScheduleService.Application.Common.Specifications.ClassEntity;
using ScheduleService.Application.Contracts.Services;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Teacher;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.GetClassesOnCurrentDateForTeacher;

public class GetClassesOnCurrentDateForTeacherQueryHandler(IClassService classService)
    : IRequestHandler<
        GetClassesOnCurrentDateForTeacherQuery,
        GetClassesOnCurrentDateForTeacherResponse
    >
{
    private readonly IClassService _classService = classService;

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

        return new GetClassesOnCurrentDateForTeacherResponse
        {
            Classes = classes,
            Weekday = weekday
        };
    }
}
