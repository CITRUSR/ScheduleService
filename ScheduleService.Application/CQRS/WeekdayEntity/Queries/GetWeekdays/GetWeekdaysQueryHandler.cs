using MediatR;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.WeekdayEntity.Queries.GetWeekdays;

public class GetWeekdaysQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetWeekdaysQuery, List<Weekday>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<List<Weekday>> Handle(
        GetWeekdaysQuery request,
        CancellationToken cancellationToken
    )
    {
        var weekdays = await _unitOfWork.WeekdayRepository.GetAllAsync();

        return weekdays;
    }
}
