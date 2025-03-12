using MediatR;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.Contracts.Helpers;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.SubjectEntity.Commands.CreateSubject;

public class CreateSubjectCommandHandler(
    IUnitOfWork unitOfWork,
    IUniqueConstraintExceptionChecker uniqueChecker
) : IRequestHandler<CreateSubjectCommand, Subject>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUniqueConstraintExceptionChecker _uniqueChecker = uniqueChecker;

    public async Task<Subject> Handle(
        CreateSubjectCommand request,
        CancellationToken cancellationToken
    )
    {
        var subject = new Subject() { Name = request.Name, Abbreviation = request.Abbreviation };

        Subject createdSubject;

        try
        {
            createdSubject = await _unitOfWork.SubjectRepository.InsertAsync(subject);

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

        return createdSubject;
    }
}
