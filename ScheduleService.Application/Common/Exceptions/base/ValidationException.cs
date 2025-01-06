namespace ScheduleService.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public ValidationError Errors { get; set; }

    public ValidationException(ValidationError errors)
    {
        Errors = errors;
    }
}
