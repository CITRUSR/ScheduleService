using Grpc.Core;
using Mapster;
using MediatR;
using ScheduleService.Application.CQRS.ClassEntity.Commands.CreateClass;
using ScheduleService.Application.CQRS.ClassEntity.Commands.DeleteClass;
using ScheduleService.Application.CQRS.ClassEntity.Commands.UpdateClass;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClassById;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.GetClassesOnCurrentDateForTeacher;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student.GetClassesForWeekForStudent;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student.GetClassOnCurrentDateForStudents;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Teacher.GetClassesForWeekForTeacher;

namespace ScheduleService.API.Services;

public class ClassService(IMediator mediator) : ScheduleService.ClassService.ClassServiceBase
{
    private readonly IMediator _mediator = mediator;

    public override async Task<CreateClassResponse> CreateClass(
        CreateClassRequest request,
        ServerCallContext context
    )
    {
        var command = request.Adapt<CreateClassCommand>();

        var @class = await _mediator.Send(command);

        return new CreateClassResponse { Class = @class.Adapt<Class>() };
    }

    public override async Task<UpdateClassResponse> UpdateClass(
        UpdateClassRequest request,
        ServerCallContext context
    )
    {
        var command = request.Adapt<UpdateClassCommand>();

        var @class = await _mediator.Send(command);

        return new UpdateClassResponse { Class = @class.Adapt<Class>() };
    }

    public override async Task<GetClassByIdResponse> GetClassById(
        GetClassByIdRequest request,
        ServerCallContext context
    )
    {
        var query = request.Adapt<GetClassByIdQuery>();

        var @class = await _mediator.Send(query);

        return new GetClassByIdResponse { Class = @class.Adapt<Class>() };
    }

    public override async Task<DeleteClassResponse> DeleteClass(
        DeleteClassRequest request,
        ServerCallContext context
    )
    {
        var command = request.Adapt<DeleteClassCommand>();

        var @class = await _mediator.Send(command);

        return new DeleteClassResponse { Class = @class.Adapt<Class>() };
    }

    public override async Task<GetClassesOnCurrentDateForStudentResponse> GetClassesOnCurrentDateForStudent(
        GetClassesOnCurrentDateRequest request,
        ServerCallContext context
    )
    {
        var query = request.Adapt<GetClassesOnCurrentDateForStudentQuery>();

        var result = await _mediator.Send(query);

        return new GetClassesOnCurrentDateForStudentResponse
        {
            Classes = { result.Classes.Adapt<List<StudentColorClasses>>() },
            Group = result.Group.Adapt<GroupViewModel>(),
            Weekday = result.Weekday.Adapt<Weekday>()
        };
    }

    public override async Task<GetClassesOnCurrentDateForTeacherResponse> GetClassesOnCurrentDateForTeacher(
        GetClassesOnCurrentDateForTeacherRequest request,
        ServerCallContext context
    )
    {
        var query = request.Adapt<GetClassesOnCurrentDateForTeacherQuery>();

        var result = await _mediator.Send(query);

        return new GetClassesOnCurrentDateForTeacherResponse
        {
            Classes = { result.Classes.Adapt<List<TeacherColorClasses>>(), },
            Weekday = result.Weekday.Adapt<Weekday>(),
            Teacher = result.Teacher.Adapt<TeacherViewModel>()
        };
    }

    public override async Task<GetClassesForWeekForStudentResponse> GetClassesForWeekForStudent(
        GetClassesForWeekForStudentRequest request,
        ServerCallContext context
    )
    {
        var query = request.Adapt<GetClassesForWeekForStudentQuery>();

        var result = await _mediator.Send(query);

        return new GetClassesForWeekForStudentResponse
        {
            Classes = { result.Classes.Adapt<List<StudentWeekdayColorClassesDto>>() },
            Group = result.Group.Adapt<GroupViewModel>()
        };
    }

    public override async Task<GetClassesForWeekForTeacherResponse> GetClassesForWeekForTeacher(
        GetClassesForWeekForTeacherRequest request,
        ServerCallContext context
    )
    {
        var query = request.Adapt<GetClassesForWeekForTeacherQuery>();

        var result = await _mediator.Send(query);

        return new GetClassesForWeekForTeacherResponse
        {
            Classes = { result.Classes.Adapt<List<TeacherWeekdayColorClassesDto>>() },
            Teacher = result.Teacher.Adapt<TeacherViewModel>()
        };
    }
}
