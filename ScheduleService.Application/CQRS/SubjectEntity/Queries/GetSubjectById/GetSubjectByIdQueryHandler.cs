using MediatR;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.SubjectEntity.Queries.GetSubjectById;

public class GetSubjectByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetSubjectByIdQuery, Subject>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Subject> Handle(
        GetSubjectByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var subject = await _unitOfWork.SubjectRepository.GetByIdAsync(request.Id);

        if (subject == null)
        {
            throw new SubjectNotFoundException(request.Id);
        }

        return subject;
    }
}
