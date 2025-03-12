using ScheduleService.Domain.Attributes;

namespace ScheduleService.Domain.Entities;

public class Color : BaseModel
{
    public int Id { get; set; }

    [Unique("name")]
    public string Name { get; set; }
}
