using ScheduleService.Application.Contracts.UserService.Group.Dto.Responses;

namespace ScheduleService.Application.Contracts.UserService.Group;

public interface IGroupService
{
    public Task<GroupDto> GetGroupById(int id);
}
