namespace ScheduleService.Application.Common.Exceptions.CurrentWeekdayEntity;

public class CurrentWeekdayNotFoundException : NotFoundException
{
    public CurrentWeekdayNotFoundException()
        : base("Current weekday not found") { }
}
