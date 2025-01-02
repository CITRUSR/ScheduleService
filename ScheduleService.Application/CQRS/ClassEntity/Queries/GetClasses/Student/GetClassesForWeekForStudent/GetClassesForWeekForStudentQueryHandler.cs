using MediatR;
using ScheduleService.Application.Common.Specifications.ClassEntity;
using ScheduleService.Application.Contracts.Services;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student.GetClassesForWeekForStudent;

public class GetClassesForWeekForStudentQueryHandler(IClassService classService)
    : IRequestHandler<GetClassesForWeekForStudentQuery, GetClassesForWeekForStudentResponse>
{
    private readonly IClassService _classService = classService;

    public async Task<GetClassesForWeekForStudentResponse> Handle(
        GetClassesForWeekForStudentQuery request,
        CancellationToken cancellationToken
    )
    {
        var classes = await _classService.GetClassesForWeek<StudentClassDetailDto>(
            new GetClassesForWeekForStudentSpecification(request.GroupId)
        );

        return new GetClassesForWeekForStudentResponse
        {
            GroupId = request.GroupId,
            Classes = classes
        };
    }
}
