using MediatR;
using ScheduleService.Application.Common.Exceptions.CurrentWeekdayEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.CurrentWeekdayEntity.Queries.GetCurrentWeekday;

public class GetCurrentWeekdayQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetCurrentWeekdayQuery, CurrentWeekday>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CurrentWeekday> Handle(
        GetCurrentWeekdayQuery request,
        CancellationToken cancellationToken
    )
    {
        var currentWeekday = await _unitOfWork.CurrentWeekdayRepository.GetAsync();

        if (currentWeekday == null)
        {
            throw new CurrentWeekdayNotFoundException();
        }

        return currentWeekday;
    }
}
