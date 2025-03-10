using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Mapster;
using MediatR;
using ScheduleService.Application.CQRS.SpecialityTeacherSubjectEntity.Commands.CreateSpecialityTeacherSubject;
using ScheduleService.Application.CQRS.SpecialityTeacherSubjectEntity.Commands.DeleteSpecialityTeacherSubject;
using ScheduleService.Application.CQRS.SpecialityTeacherSubjectEntity.Queries.GetAllSpecialityTeacherSubjects;
using ScheduleService.Application.CQRS.SpecialityTeacherSubjectEntity.Queries.GetSpecialityTeacherSubjectById;

namespace ScheduleService.API.Services;

public class SpecialityTeacherSubjectService(IMediator mediator)
    : ScheduleService.SpecialityTeacherSubjectService.SpecialityTeacherSubjectServiceBase
{
    private readonly IMediator _mediator = mediator;

    public override async Task<GetSpecialityTeacherSubjectsResponse> GetAllSpecialityTeacherSubjects(
        Empty request,
        ServerCallContext context
    )
    {
        var entities = await _mediator.Send(new GetAllSpecialityTeacherSubjectsQuery());

        return new GetSpecialityTeacherSubjectsResponse
        {
            SpecialityTeacherSubjects = { entities.Adapt<List<SpecialityTeacherSubjectModel>>() },
        };
    }

    public override async Task<SpecialityTeacherSubjectModel> GetSpecialityTeacherSubjectById(
        GetSpecialityTeacherSubjectByIdRequest request,
        ServerCallContext context
    )
    {
        var query = request.Adapt<GetSpecialityTeacherSubjectByIdQuery>();

        var entity = await _mediator.Send(query);

        return entity.Adapt<SpecialityTeacherSubjectModel>();
    }

    public override async Task<SpecialityTeacherSubjectModel> CreateSpecialityTeacherSubject(
        CreateSpecialityTeacherSubjectRequest request,
        ServerCallContext context
    )
    {
        var command = request.Adapt<CreateSpecialityTeacherSubjectCommand>();

        var res = await _mediator.Send(command);

        return res.Adapt<SpecialityTeacherSubjectModel>();
    }

    public override async Task<SpecialityTeacherSubjectModel> DeleteSpecialityTeacherSubject(
        DeleteSpecialityTeacherSubjectRequest request,
        ServerCallContext context
    )
    {
        var command = request.Adapt<DeleteSpecialityTeacherSubjectCommand>();

        var res = await _mediator.Send(command);

        return res.Adapt<SpecialityTeacherSubjectModel>();
    }
}
