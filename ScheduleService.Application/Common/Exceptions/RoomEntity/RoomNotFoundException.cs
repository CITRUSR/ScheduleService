namespace ScheduleService.Application.Common.Exceptions;

public class RoomNotFoundException : NotFoundException
{
    public RoomNotFoundException(int id)
        : base($"Room with id {id} not found") { }
}
