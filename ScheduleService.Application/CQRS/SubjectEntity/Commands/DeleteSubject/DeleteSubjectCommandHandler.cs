using MediatR;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.SubjectEntity.Commands.DeleteSubject;

public class DeleteSubjectCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteSubjectCommand, Subject>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Subject> Handle(
        DeleteSubjectCommand request,
        CancellationToken cancellationToken
    )
    {
        var subject = await _unitOfWork.SubjectRepository.DeleteAsync(request.Id);

        if (subject == null)
        {
            throw new SubjectNotFoundException(request.Id);
        }

        _unitOfWork.CommitTransaction();

        return subject;
    }
}
