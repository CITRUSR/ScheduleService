namespace ScheduleService.Application.Common.Exceptions;

public class RoomNotFoundException : Exception
{
    public RoomNotFoundException(int id)
        : base($"Room with id {id} not found") { }
}
