using ScheduleService.Domain.Attributes;

namespace ScheduleService.Domain.Entities;

public class SpecialityTeacherSubject : BaseModel
{
    [Unique("speciality_fk")]
    public int SpecialityId { get; set; }

    [Unique("course")]
    public int Course { get; set; }

    [Unique("subgroup")]
    public int SubGroup { get; set; }

    [Unique("teacher_fk")]
    public Guid TeacherId { get; set; }

    [Unique("subject_fk")]
    public int SubjectId { get; set; }
}
