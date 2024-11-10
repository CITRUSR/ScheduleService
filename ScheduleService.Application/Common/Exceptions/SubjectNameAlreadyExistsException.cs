namespace ScheduleService.Application.Common.Exceptions;

public class SubjectNameAlreadyExistsException : Exception
{
    public SubjectNameAlreadyExistsException(string name)
        : base($"the subject with name '{name}' already exists") { }
}
