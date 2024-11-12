namespace ScheduleService.Application.Common.Exceptions;

public class WeekdayNotFoundException : Exception
{
    public WeekdayNotFoundException(int id)
        : base($"weekday with id {id} not found") { }
}
