using MediatR;
using ScheduleService.Application.Common.Extensions;
using ScheduleService.Application.Common.Specifications.ClassEntity;
using ScheduleService.Application.Contracts.Services;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student.GetClassOnCurrentDateForStudents;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.GetClassOnCurrentDateForStudents;

public class GetClassesOnCurrentDateForStudentQueryHandler(IClassService classService)
    : IRequestHandler<
        GetClassesOnCurrentDateForStudentQuery,
        GetClassesOnCurrentDateForStudentResponse
    >
{
    private readonly IClassService _classService = classService;

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

        var result = new GetClassesOnCurrentDateForStudentResponse
        {
            Classes = classes,
            GroupId = request.GroupId,
            Weekday = weekday
        };

        return result;
    }
}
