using Grpc.Core;
using Mapster;
using MediatR;
using ScheduleService.Application.CQRS.SubjectEntity.Commands.CreateSubject;

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
}
