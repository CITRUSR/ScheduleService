namespace ScheduleService.Application.Common.Exceptions;

public class ColorNotFoundException : NotFoundException
{
    public ColorNotFoundException(int id)
        : base($"color with id {id} not found") { }
}
