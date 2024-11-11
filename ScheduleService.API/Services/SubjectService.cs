using Grpc.Core;
using Mapster;
using MediatR;
using ScheduleService.Application.CQRS.SubjectEntity.Commands.CreateSubject;
using ScheduleService.Application.CQRS.SubjectEntity.Commands.DeleteSubject;
using ScheduleService.Application.CQRS.SubjectEntity.Commands.UpdateSubject;
using ScheduleService.Application.CQRS.SubjectEntity.Queries.GetSubjectById;

namespace ScheduleService.API.Services;

public class SubjectService(IMediator mediator) : ScheduleService.SubjectService.SubjectServiceBase
{
    private readonly IMediator _mediator = mediator;

    public override async Task<CreateSubjectResponse> CreateSubject(
        CreateSubjectRequest request,
        ServerCallContext context
    )
    {
        var command = request.Adapt<CreateSubjectCommand>();

        var subject = await _mediator.Send(command);

        return new CreateSubjectResponse() { Subject = subject.Adapt<Subject>() };
    }

    public override async Task<GetSubjectByIdResponse> GetSubjectById(
        GetSubjectByIdRequest request,
        ServerCallContext context
    )
    {
        var query = request.Adapt<GetSubjectByIdQuery>();

        var subject = await _mediator.Send(query);

        return new GetSubjectByIdResponse() { Subject = subject.Adapt<Subject>() };
    }

    public override async Task<UpdateSubjectResponse> UpdateSubject(
        UpdateSubjectRequest request,
        ServerCallContext context
    )
    {
        var command = request.Adapt<UpdateSubjectCommand>();

        var subject = await _mediator.Send(command);

        return new UpdateSubjectResponse() { Subject = subject.Adapt<Subject>() };
    }

    public override async Task<DeleteSubjectResponse> DeleteSubject(
        DeleteSubjectRequest request,
        ServerCallContext context
    )
    {
        var command = request.Adapt<DeleteSubjectCommand>();

        var subject = await _mediator.Send(command);

        return new DeleteSubjectResponse() { Subject = subject.Adapt<Subject>() };
    }
}
