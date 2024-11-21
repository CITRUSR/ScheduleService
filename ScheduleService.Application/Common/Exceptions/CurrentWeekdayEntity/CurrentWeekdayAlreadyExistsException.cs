namespace ScheduleService.Application.Common.Exceptions.CurrentWeekdayEntity;

public class CurrentWeekdayAlreadyExistsException : AlreadyExistsException
{
    public CurrentWeekdayAlreadyExistsException()
        : base("Current weekday already exists") { }
}
