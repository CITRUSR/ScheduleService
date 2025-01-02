using MediatR;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.Contracts.UserService.Group;
using ScheduleService.Application.Contracts.UserService.Group.Dto.Responses;
using ScheduleService.Application.Contracts.UserService.Teacher;
using ScheduleService.Application.Contracts.UserService.Teacher.dto.responses;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.ClassEntity.Commands.CreateClass;

public class CreateClassCommandHandler(
    IUnitOfWork unitOfWork,
    IGroupService groupService,
    ITeacherService teacherService
) : IRequestHandler<CreateClassCommand, Class>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ITeacherService _teacherService = teacherService;
    private readonly IGroupService _groupService = groupService;

    public async Task<Class> Handle(CreateClassCommand request, CancellationToken cancellationToken)
    {
        CreateClassDto dto = new CreateClassDto(
            request.GroupId,
            request.SubjectId,
            request.WeekdayId,
            request.ColorId,
            request.StartsAt,
            request.EndsAt,
            request.ChangeOn,
            request.TeachersIds,
            request.RoomIds
        );

        GetClassDependenciesDto dependnceDto = new GetClassDependenciesDto(
            request.GroupId,
            request.SubjectId,
            request.WeekdayId,
            request.ColorId,
            request.RoomIds
        );

        var req = await _unitOfWork.ClassRepository.GetClassDependencies(dependnceDto);

        if (req.Color == null && request.ColorId.HasValue)
            throw new ColorNotFoundException(request.ColorId.Value);

        if (req.Subject == null)
            throw new SubjectNotFoundException(request.SubjectId);

        if (req.Weekday == null)
            throw new WeekdayNotFoundException(request.WeekdayId);

        if (req.Rooms.Count != request.RoomIds.Count)
        {
            var notFoundIds = request.RoomIds.Except(req.Rooms.Select(x => x.Id));
            throw new RoomNotFoundException([.. notFoundIds]);
        }

        //if teachers not found will be rpc exception

        List<Task<TeacherDto>> getTeacherTasks = [];

        foreach (var teacher in request.TeachersIds)
        {
            getTeacherTasks.Add(_teacherService.GetTeacherById(teacher));
        }

        //if group not found will be rpc exception

        Task<GroupDto> getGroupTask = _groupService.GetGroupById(request.GroupId);

        await Task.WhenAll([.. getTeacherTasks, getGroupTask]);

        var result = await _unitOfWork.ClassRepository.InsertAsync(dto);

        _unitOfWork.CommitTransaction();

        return result;
    }
}
