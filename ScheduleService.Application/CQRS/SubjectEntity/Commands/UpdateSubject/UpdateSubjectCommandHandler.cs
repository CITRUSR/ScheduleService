using MediatR;
using Npgsql;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.SubjectEntity.Commands.UpdateSubject;

public class UpdateSubjectCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateSubjectCommand, Subject>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Subject> Handle(
        UpdateSubjectCommand request,
        CancellationToken cancellationToken
    )
    {
        var subject = new Subject()
        {
            Id = request.Id,
            Name = request.Name,
            Abbreviation = request.Abbreviation
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
        catch (NpgsqlException ex) when (ex.SqlState == "23505")
        {
            _unitOfWork.RollbackTransaction();

            throw new SubjectNameAlreadyExistsException(request.Name);
        }
        catch
        {
            _unitOfWork.RollbackTransaction();
            throw;
        }

        return updatedSubject;
    }
}
