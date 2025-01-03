namespace ScheduleService.Application.Common.Exceptions;

public class RoomNotFoundException : NotFoundException
{
    public RoomNotFoundException(int id)
        : base($"Room with id {id} not found") { }

    public RoomNotFoundException(int[] id)
        : base($"Rooms with id {string.Join(", ", id)} not found") { }
}
