using MediatR;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.Contracts.Helpers;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.SubjectEntity.Commands.UpdateSubject;

public class UpdateSubjectCommandHandler(
    IUnitOfWork unitOfWork,
    IUniqueConstraintExceptionChecker uniqueChecker
) : IRequestHandler<UpdateSubjectCommand, Subject>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUniqueConstraintExceptionChecker _uniqueChecker = uniqueChecker;

    public async Task<Subject> Handle(
        UpdateSubjectCommand request,
        CancellationToken cancellationToken
    )
    {
        var subject = new Subject()
        {
            Id = request.Id,
            Name = request.Name,
            Abbreviation = request.Abbreviation,
        };

        Subject updatedSubject;

        try
        {
            updatedSubject = await _unitOfWork.SubjectRepository.UpdateAsync(subject);

            if (updatedSubject == null)
            {
                throw new SubjectNotFoundException(request.Id);
            }

            _unitOfWork.CommitTransaction();
        }
        catch (Exception ex)
        {
            _unitOfWork.RollbackTransaction();

            var prop = _uniqueChecker.Check<Subject>(ex);

            if (prop != null && prop.Equals("name", StringComparison.OrdinalIgnoreCase))
            {
                throw new SubjectNameAlreadyExistsException(request.Name);
            }
            throw;
        }

        return updatedSubject;
    }
}
