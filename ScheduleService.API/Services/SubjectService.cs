using Grpc.Core;
using Mapster;
using MediatR;
using ScheduleService.Application.CQRS.SubjectEntity.Commands.CreateSubject;
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
}
