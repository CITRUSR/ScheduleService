using MediatR;

namespace ScheduleService.Application.CQRS.CurrentWeekdayEntity.Commands.CreateCurrentWeekday;

public class CreateCurrentWeekdayEvent : INotification
{
    public string Color { get; set; }
    public TimeSpan Interval { get; set; }
    public DateTime UpdateTime { get; set; }
}
