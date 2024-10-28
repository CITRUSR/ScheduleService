namespace ScheduleService.Application.Common.Exceptions;

public class ValidationError
{
    public string PropertyName { get; set; }
    protected string ErrorMessage { get; set; }
}
