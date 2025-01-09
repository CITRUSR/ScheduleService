using ScheduleService.Application.Contracts.UserService.Group.Dto.Responses;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student.GetClassesForWeekForStudent;

public class GetClassesForWeekForStudentResponse
{
    public GroupViewModel Group { get; set; }
    public List<
        WeekdayColorClassesDto<ColorClassesDto<StudentClassDetailDto>, StudentClassDetailDto>
    > Classes { get; set; }
}
