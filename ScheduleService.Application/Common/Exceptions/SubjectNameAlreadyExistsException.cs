namespace ScheduleService.Application.Common.Exceptions;

public class SubjectNameAlreadyExistsException : AlreadyExistsException
{
    public SubjectNameAlreadyExistsException(string name)
        : base($"the subject with name '{name}' already exists") { }
}
