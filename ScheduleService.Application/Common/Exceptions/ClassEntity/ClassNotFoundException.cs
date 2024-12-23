namespace ScheduleService.Application.Common.Exceptions.ClassEntity;

public class ClassNotFoundException : NotFoundException
{
    public ClassNotFoundException(int id)
        : base($"class with id:{id} not found") { }
}
