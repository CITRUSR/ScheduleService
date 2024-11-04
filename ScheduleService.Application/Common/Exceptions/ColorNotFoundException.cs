namespace ScheduleService.Application.Common.Exceptions;
public class ColorNotFoundException : Exception{
    public ColorNotFoundException(int id) : base($"Color with id {id} not found.")
    {
        
    }
}