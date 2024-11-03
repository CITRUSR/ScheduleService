namespace ScheduleService.Domain.Entities;

public class SpecialityTeacherSubject
{
    public int SpecialityId { get; set; }
    public int Course { get; set; }
    public int SubGroup { get; set; }
    public Guid TeacherId { get; set; }
    public Subject Subject { get; set; }
}
