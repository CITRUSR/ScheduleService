using MediatR;
using ScheduleService.Application.Common.Specifications.ClassEntity;
using ScheduleService.Application.Contracts.Services;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Teacher.GetClassesForWeekForTeacher;

public class GetClassesForWeekForTeacherQueryHandler(IClassService classService)
    : IRequestHandler<GetClassesForWeekForTeacherQuery, GetClassesForWeekForTeacherResponse>
{
    private readonly IClassService _classService = classService;

    public async Task<GetClassesForWeekForTeacherResponse> Handle(
        GetClassesForWeekForTeacherQuery request,
        CancellationToken cancellationToken
    )
    {
        var classes = await _classService.GetClassesForWeek<TeacherClassDetailDto>(
            new GetClassesForWeekForTeacherSpecification(request.TeacherId)
        );

        return new GetClassesForWeekForTeacherResponse
        {
            Classes = classes,
            TeacherId = request.TeacherId
        };
    }
}
