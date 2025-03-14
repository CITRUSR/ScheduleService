using MediatR;
using ScheduleService.Application.Common.Models;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.SubjectEntity.Queries.GetSubjects;

public class GetSubjectsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetSubjectsQuery, PagedList<Subject>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<PagedList<Subject>> Handle(
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
