namespace ScheduleService.Domain.Entities;

public class Subject : BaseModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Abbreviation { get; set; }
}
