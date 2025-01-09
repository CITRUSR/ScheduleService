using ScheduleService.Application.Contracts.UserService.Group.Dto.Responses;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Teacher;

public class TeacherClassDetailDto : ClassDetailBase
{
    public int GroupId { get; set; }
    public GroupViewModel Group { get; set; }
}
