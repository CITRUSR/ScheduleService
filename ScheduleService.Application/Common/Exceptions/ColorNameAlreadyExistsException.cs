namespace ScheduleService.Application.Common.Exceptions;

public class ColorNameAlreadyExistsException : Exception
{
    public ColorNameAlreadyExistsException(string name)
        : base($"the color with name '{name}' already exists") { }
}
