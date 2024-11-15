using MediatR;
using ScheduleService.Application.Common.Exceptions.CurrentWeekdayEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.CurrentWeekdayEntity.Commands.UpdateCurrentWeekday;

public class UpdateCurrentWeekdayCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateCurrentWeekdayCommand, CurrentWeekday>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CurrentWeekday> Handle(
        UpdateCurrentWeekdayCommand request,
        CancellationToken cancellationToken
    )
    {
        var newCurrentWeekday = new CurrentWeekday
        {
            Color = request.Color,
            Interval = request.Interval,
            UpdatedAt = request.UpdateTime
        };

        var currentWeekday = await _unitOfWork.CurrentWeekdayRepository.UpdateAsync(
            newCurrentWeekday
        );

        if (currentWeekday == null)
        {
            throw new CurrentWeekdayNotFoundException();
        }

        _unitOfWork.CommitTransaction();

        return currentWeekday;
    }
}
