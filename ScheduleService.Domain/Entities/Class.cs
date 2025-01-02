namespace ScheduleService.Domain.Entities;

public class Class
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public Subject Subject { get; set; }
    public Weekday Weekday { get; set; }
    public Color? Color { get; set; }
    public TimeSpan StartsAt { get; set; }
    public TimeSpan EndsAt { get; set; }
    public DateTime? ChangeOn { get; set; }
    public DateTime? IrrelevantSince { get; set; }
    public List<Guid> TeacherIds { get; set; } = [];
    public List<Room> Rooms { get; set; } = [];
}
