namespace ScheduleService.Application.Common.Exceptions;

public class WeekdayNotFoundException : NotFoundException
{
    public WeekdayNotFoundException(int id)
        : base($"weekday with id {id} not found") { }
}
