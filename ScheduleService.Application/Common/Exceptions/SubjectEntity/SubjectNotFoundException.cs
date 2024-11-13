namespace ScheduleService.Application.Common.Exceptions;

public class SubjectNotFoundException : NotFoundException
{
    public SubjectNotFoundException(int id)
        : base($"subject with id {id} not found") { }
}
