using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Mapster;
using MediatR;
using ScheduleService.Application.CQRS.SpecialityTeacherSubjectEntity.Queries.GetAllSpecialityTeacherSubjects;

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
}
