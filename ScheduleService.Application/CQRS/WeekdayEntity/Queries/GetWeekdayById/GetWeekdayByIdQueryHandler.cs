using MediatR;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.WeekdayEntity.Queries.GetWeekdayById;

public class GetWeekdayByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetWeekdayByIdQuery, Weekday>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Weekday> Handle(
        GetWeekdayByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var weekday = await _unitOfWork.WeekdayRepository.GetByIdAsync(request.Id);

        if (weekday == null)
        {
            throw new WeekdayNotFoundException(request.Id);
        }

        return weekday;
    }
}
