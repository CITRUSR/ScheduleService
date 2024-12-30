using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses;

public class ClassDetailBase
{
    public int Id { get; set; }
    public int Order { get; set; }
    public Subject Subject { get; set; }
    public TimeSpan StartsAt { get; set; }
    public TimeSpan EndsAt { get; set; }
    public DateTime? ChangeOn { get; set; }
    public List<Room> Rooms { get; set; }
};
