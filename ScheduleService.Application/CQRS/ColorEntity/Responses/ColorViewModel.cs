namespace ScheduleService.Application.CQRS.ColorEntity.Responses;

public record ColorViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
}
