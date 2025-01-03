using MediatR;

namespace ScheduleService.Application.CQRS.CurrentWeekdayEntity.Commands.UpdateCurrentWeekday;

public class UpdateCurrentWeekdayEvent : INotification
{
    public string Color { get; set; }
    public TimeSpan Interval { get; set; }
    public DateTime UpdateTime { get; set; }
}
