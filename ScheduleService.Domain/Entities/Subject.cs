using ScheduleService.Domain.Attributes;

namespace ScheduleService.Domain.Entities;

public class Subject : BaseModel
{
    public int Id { get; set; }

    [Unique("name")]
    public string Name { get; set; }
    public string? Abbreviation { get; set; }
}
