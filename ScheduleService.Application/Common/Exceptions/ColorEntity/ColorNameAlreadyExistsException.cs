namespace ScheduleService.Application.Common.Exceptions;

public class ColorNameAlreadyExistsException : AlreadyExistsException
{
    public ColorNameAlreadyExistsException(string name)
        : base($"the color with name '{name}' already exists") { }
}
