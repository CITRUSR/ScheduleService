using Mapster;
using MediatR;
using ScheduleService.Application.Common.Extensions;
using ScheduleService.Application.Common.Specifications.ClassEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Teacher;
using ScheduleService.Application.CQRS.WeekdayEntity.Queries.GetWeekdayById;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.GetClassesOnCurrentDateForTeacher;

public class GetClassesOnCurrentDateForTeacherQueryHandler(
    IRequestHandler<GetWeekdayByIdQuery, Weekday> weekdayHandler,
    IUnitOfWork unitOfWork
)
    : IRequestHandler<
        GetClassesOnCurrentDateForTeacherQuery,
        GetClassesOnCurrentDateForTeacherResponse
    >
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IRequestHandler<GetWeekdayByIdQuery, Weekday> _weekdayHandler = weekdayHandler;

    public async Task<GetClassesOnCurrentDateForTeacherResponse> Handle(
        GetClassesOnCurrentDateForTeacherQuery request,
        CancellationToken cancellationToken
    )
    {
        int weekdayId = DateTime.Now.GetCurrentWeekdayOrder() + 1;

        var weekday = await _weekdayHandler.Handle(
            new GetWeekdayByIdQuery(weekdayId),
            cancellationToken
        );

        var classes = await _unitOfWork.ClassRepository.GetAsync(
            new GetClassesOnCurrentDateForTeacherSpecification(request.TeacherId, weekdayId)
        );

        var colorClasses = classes.ToColorClasses<TeacherClassDetailDto>();

        colorClasses.CountClassOrder();

        return new GetClassesOnCurrentDateForTeacherResponse
        {
            Classes = colorClasses,
            Weekday = weekday
        };
    }
}
