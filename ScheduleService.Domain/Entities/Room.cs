namespace ScheduleService.Domain.Entities;

public class Room : BaseModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? FullName { get; set; }
}
