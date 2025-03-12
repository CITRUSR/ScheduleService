namespace ScheduleService.Domain.Entities;

public class SpecialityTeacherSubject : BaseModel
{
    public int SpecialityId { get; set; }
    public int Course { get; set; }
    public int SubGroup { get; set; }
    public Guid TeacherId { get; set; }
    public int SubjectId { get; set; }
}
