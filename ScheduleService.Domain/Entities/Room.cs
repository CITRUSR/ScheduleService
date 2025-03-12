using ScheduleService.Domain.Attributes;

namespace ScheduleService.Domain.Entities;

public class Room : BaseModel
{
    public int Id { get; set; }

    [Unique("name")]
    public string Name { get; set; }
    public string? FullName { get; set; }
}
