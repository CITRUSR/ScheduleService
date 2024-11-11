using MediatR;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.SubjectEntity.Queries.GetSubjects;

public class GetSubjectsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetSubjectsQuery, List<Subject>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<List<Subject>> Handle(
        GetSubjectsQuery request,
        CancellationToken cancellationToken
    )
    {
        var subjects = await _unitOfWork.SubjectRepository.GetAsync(
            request.Filter,
            request.PaginationParameters
        );

        return subjects;
    }
}
