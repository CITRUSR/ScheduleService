namespace ScheduleService.Domain.Entities;

public class CurrentWeekday : BaseModel
{
    public int Id { get; set; }
    public string Color { get; set; }
    public TimeSpan Interval { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
