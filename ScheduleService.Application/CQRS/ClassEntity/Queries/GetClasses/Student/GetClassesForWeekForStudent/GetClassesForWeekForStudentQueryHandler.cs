using Mapster;
using MediatR;
using ScheduleService.Application.Common.Extensions;
using ScheduleService.Application.Common.Specifications.ClassEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.WeekdayEntity.Queries.GetWeekdayById;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student.GetClassesForWeekForStudent;

public class GetClassesForWeekForStudentQueryHandler(
    IUnitOfWork unitOfWork,
    IRequestHandler<GetWeekdayByIdQuery, Weekday> weekdayHandler
) : IRequestHandler<GetClassesForWeekForStudentQuery, GetClassesForWeekForStudentResponse>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IRequestHandler<GetWeekdayByIdQuery, Weekday> _weekdayHandler = weekdayHandler;

    public async Task<GetClassesForWeekForStudentResponse> Handle(
        GetClassesForWeekForStudentQuery request,
        CancellationToken cancellationToken
    )
    {
        var currentWeekdayOrder = DateTime.Now.GetCurrentWeekdayOrder();

        var leftBorder = DateTime.Now.AddDays(-currentWeekdayOrder);

        var rightBorder = leftBorder.AddDays(5);

        List<Weekday> weekdays = [];

        for (var i = 1; i <= 7; i++)
        {
            var weekday = await _weekdayHandler.Handle(
                new GetWeekdayByIdQuery(i),
                cancellationToken
            );

            weekdays.Add(weekday);
        }

        var classes = await _unitOfWork.ClassRepository.GetAsync(
            new GetClassesForWeekForStudentSpecification(leftBorder, rightBorder, request.GroupId)
        );

        var weekdayClasses = classes
            .GroupBy(x => x.Weekday.Id)
            .Select(x => new WeekdayColorClassesDto<
                ColorClassesDto<StudentClassDetailDto>,
                StudentClassDetailDto
            >()
            {
                Weekday = weekdays[x.Key - 1],
                Classes = x.ToList().ToColorClasses<StudentClassDetailDto>()
            })
            .ToList();

        for (int i = 0; i < weekdayClasses.Count; i++)
        {
            weekdayClasses[i].Classes.CountClassOrder();
        }

        return new GetClassesForWeekForStudentResponse
        {
            GroupId = request.GroupId,
            Classes = [.. weekdayClasses]
        };
    }
}
