namespace ScheduleService.Application.Contracts.UserService.Group.Dto.Responses;

public record GroupDto
{
    public int Id { get; set; }
    public int SpecialityId { get; set; }
    public string CuratorId { get; set; }
    public byte CurrentCourse { get; set; }
    public byte CurrentSemester { get; set; }
    public byte SubGroup { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? GraduatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public string FullName { get; set; }
}
