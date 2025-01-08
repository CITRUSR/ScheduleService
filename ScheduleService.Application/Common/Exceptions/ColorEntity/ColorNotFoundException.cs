namespace ScheduleService.Application.Common.Exceptions;

public class ColorNotFoundException : NotFoundException
{
    public ColorNotFoundException(int id)
        : base($"color with id {id} not found") { }

    public ColorNotFoundException(string name)
        : base($"color with name '{name}' not found") { }
}
