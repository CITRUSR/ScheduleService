using Mapster;
using MediatR;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Common.Exceptions.ClassEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.Contracts.UserService.Group;
using ScheduleService.Application.Contracts.UserService.Teacher;
using ScheduleService.Application.Contracts.UserService.Teacher.dto.responses;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClassById;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.ClassEntity.Commands.UpdateClass;

public class UpdateClassCommandHandler(
    IUnitOfWork unitOfWork,
    IGroupService groupService,
    ITeacherService teacherService,
    IMediator mediator
) : IRequestHandler<UpdateClassCommand, Class>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IGroupService _groupService = groupService;
    private readonly ITeacherService _teacherService = teacherService;
    private readonly IMediator _mediator = mediator;

    public async Task<Class> Handle(UpdateClassCommand request, CancellationToken cancellationToken)
    {
        var @class = await _mediator.Send(
            new GetClassByIdQuery(request.ClassId),
            cancellationToken
        );

        if (@class == null)
            throw new ClassNotFoundException(request.ClassId);

        var depenceDto = request.Adapt<GetClassDependenciesDto>();

        var dependencies = await _unitOfWork.ClassRepository.GetClassDependencies(depenceDto);

        if (dependencies.Subject == null)
            throw new SubjectNotFoundException(request.SubjectId);
        if (dependencies.Color == null && request.ColorId.HasValue)
            throw new ColorNotFoundException(request.ColorId.Value);
        if (dependencies.Weekday == null)
            throw new WeekdayNotFoundException(request.WeekdayId);
        if (dependencies.Rooms.Count != request.RoomIds.Count)
        {
            var notFoundIds = request.RoomIds.Except(dependencies.Rooms.Select(x => x.Id));
            throw new RoomNotFoundException([.. notFoundIds]);
        }

        //if teachers not found will be rpc exception

        var teacherTasks = new List<Task<TeacherDto>>();

        foreach (var teacherId in request.TeacherIds)
        {
            teacherTasks.Add(_teacherService.GetTeacherById(teacherId));
        }

        //if group not found will be rpc exception

        var groupTask = _groupService.GetGroupById(request.GroupId);

        await Task.WhenAll([.. teacherTasks, groupTask]);

        var updateClassDto = request.Adapt<UpdateClassDto>();

        var updatedClass = await _unitOfWork.ClassRepository.UpdateAsync(updateClassDto);

        _unitOfWork.CommitTransaction();

        return updatedClass;
    }
}
