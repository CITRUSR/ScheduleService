using MediatR;
using ScheduleService.Application.Contracts;

namespace ScheduleService.Application.CQRS.CurrentWeekdayEntity.Commands.CreateCurrentWeekday;

public class CreateCurrentWeekdayEventHandler(ICurrentWeekdayUpdateTaskCreator taskCreator)
    : INotificationHandler<CreateCurrentWeekdayEvent>
{
    private readonly ICurrentWeekdayUpdateTaskCreator _taskCreator = taskCreator;

    public Task Handle(CreateCurrentWeekdayEvent notification, CancellationToken cancellationToken)
    {
        _taskCreator.UpdateCurrentWeekdayTask(
            notification.Color,
            notification.Interval,
            notification.UpdateTime
        );

        return Task.CompletedTask;
    }
}
