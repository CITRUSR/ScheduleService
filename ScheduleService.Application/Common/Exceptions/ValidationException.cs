namespace ScheduleService.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public List<ValidationError> Errors { get; set; }

    public ValidationException(List<ValidationError> errors)
    {
        Errors = errors;
    }
}
