using MediatR;
using ScheduleService.Application.Contracts;

namespace ScheduleService.Application.CQRS.CurrentWeekdayEntity.Commands.UpdateCurrentWeekday;

public class UpdateCurrentWeekdayEventHandler(ICurrentWeekdayUpdateTaskCreator taskCreator)
    : INotificationHandler<UpdateCurrentWeekdayEvent>
{
    private readonly ICurrentWeekdayUpdateTaskCreator _taskCreator = taskCreator;

    public Task Handle(UpdateCurrentWeekdayEvent notification, CancellationToken cancellationToken)
    {
        _taskCreator.UpdateCurrentWeekdayTask(
            notification.Color,
            notification.Interval,
            notification.UpdateTime
        );

        return Task.CompletedTask;
    }
}
