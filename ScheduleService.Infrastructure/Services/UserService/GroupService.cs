using Mapster;
using ScheduleService.Application.Contracts.UserService.Group;
using ScheduleService.Application.Contracts.UserService.Group.Dto.Responses;

namespace ScheduleService.Infrastructure.Services.UserService;

public class GroupService(UserServiceClient.GroupService.GroupServiceClient groupService)
    : IGroupService
{
    private readonly UserServiceClient.GroupService.GroupServiceClient _groupService = groupService;

    public async Task<GroupDto> GetGroupById(int id)
    {
        var grpcRequest = new UserServiceClient.GetGroupByIdRequest { Id = id };

        var response = await _groupService.GetGroupByIdAsync(grpcRequest);

        return response.Adapt<GroupDto>();
    }
}
