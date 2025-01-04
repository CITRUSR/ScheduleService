using Mapster;
using ScheduleService.Application.Contracts.UserService.Group;
using ScheduleService.Application.Contracts.UserService.Group.Dto.Responses;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Teacher;

namespace ScheduleService.Application.Common.Extensions;

public static class TeacherClassDetailExtension
{
    public static async Task LoadGroups(
        this List<TeacherClassDetailDto> classes,
        IGroupService groupService
    )
    {
        List<(TeacherClassDetailDto @class, Task<GroupDto> groupTask)> tasks = [];

        foreach (var classDetail in classes)
        {
            tasks.Add((classDetail, groupService.GetGroupById(classDetail.GroupId)));
        }

        await Task.WhenAll(tasks.Select(x => x.groupTask));

        foreach (var (classDetail, task) in tasks)
        {
            classDetail.Group = task.Result.Adapt<GroupViewModel>();
        }
    }
}
