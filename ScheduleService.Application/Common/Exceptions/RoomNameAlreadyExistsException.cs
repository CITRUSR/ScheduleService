namespace ScheduleService.Application.Common.Exceptions;

public class RoomNameAlreadyExistsException : Exception
{
    public RoomNameAlreadyExistsException(string name)
        : base($"the room with name '{name}' already exists") { }
}