using MediatR;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Common.Exceptions.CurrentWeekdayEntity;
using ScheduleService.Application.Common.Extensions;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.ColorEntity.Queries.GetColors;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.CurrentWeekdayEntity.Commands.UpdateCurrentWeekday;

public class UpdateCurrentWeekdayCommandHandler(
    IUnitOfWork unitOfWork,
    IPublisher publisher,
    IMediator mediator
) : IRequestHandler<UpdateCurrentWeekdayCommand, CurrentWeekday>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPublisher _publisher = publisher;
    private readonly IMediator _mediator = mediator;

    public async Task<CurrentWeekday> Handle(
        UpdateCurrentWeekdayCommand request,
        CancellationToken cancellationToken
    )
    {
        var colors = await _mediator.Send(new GetColorsQuery(), cancellationToken);

        colors.EnsureColorExists(request.Color);

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

        await _publisher.Publish(
            new UpdateCurrentWeekdayEvent
            {
                Color = request.Color,
                Interval = request.Interval,
                UpdateTime = request.UpdateTime
            },
            cancellationToken
        );

        return currentWeekday;
    }
}
