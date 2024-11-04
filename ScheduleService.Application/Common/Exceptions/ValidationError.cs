namespace ScheduleService.Application.Common.Exceptions;

public class ValidationError
{
    public string PropertyName { get; set; }
    public string ErrorMessage { get; set; }
}
