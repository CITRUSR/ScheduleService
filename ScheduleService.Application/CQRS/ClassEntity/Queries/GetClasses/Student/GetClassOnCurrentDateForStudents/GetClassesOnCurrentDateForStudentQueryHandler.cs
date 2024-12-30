using Mapster;
using MediatR;
using ScheduleService.Application.Common.Extensions;
using ScheduleService.Application.Common.Specifications.ClassEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student.GetClassOnCurrentDateForStudents;
using ScheduleService.Application.CQRS.WeekdayEntity.Queries.GetWeekdayById;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.GetClassOnCurrentDateForStudents;

public class GetClassesOnCurrentDateForStudentQueryHandler(
    IUnitOfWork unitOfWork,
    IRequestHandler<GetWeekdayByIdQuery, Weekday> weekdayHandler
)
    : IRequestHandler<
        GetClassesOnCurrentDateForStudentQuery,
        GetClassesOnCurrentDateForStudentResponse
    >
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IRequestHandler<GetWeekdayByIdQuery, Weekday> _weekdayHandler = weekdayHandler;

    public async Task<GetClassesOnCurrentDateForStudentResponse> Handle(
        GetClassesOnCurrentDateForStudentQuery request,
        CancellationToken cancellationToken
    )
    {
        //order corresponds to weekday id
        //move order from 0 as sunday to 1 as monday and 7 as sunday for corresponding weekday id
        var CurrentWeekdayOrder = DateTime.Now.GetCurrentWeekdayOrder();

        var weekday = await _weekdayHandler.Handle(
            new GetWeekdayByIdQuery(CurrentWeekdayOrder),
            cancellationToken
        );

        var classes = await _unitOfWork.ClassRepository.GetAsync(
            new GetClassesOnCurrentDateForStudentSpecification(request.GroupId, CurrentWeekdayOrder)
        );

        var colorClassesDto = classes
            .GroupBy(x => x.Color)
            .Select(x => new ColorClassesDto<StudentClassDetailDto>
            {
                Color = x.Key,
                Classes = [.. x.Select(x => x.Adapt<StudentClassDetailDto>())]
            })
            .ToList();

        colorClassesDto.CountClassOrder();

        var result = new GetClassesOnCurrentDateForStudentResponse
        {
            Classes = [.. colorClassesDto],
            GroupId = request.GroupId,
            Weekday = weekday
        };

        return result;
    }
}
