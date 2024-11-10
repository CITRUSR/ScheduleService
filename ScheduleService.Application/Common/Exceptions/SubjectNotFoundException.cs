namespace ScheduleService.Application.Common.Exceptions;

public class SubjectNotFoundException : Exception
{
    public SubjectNotFoundException(int id)
        : base($"subject with id {id} not found") { }
}
