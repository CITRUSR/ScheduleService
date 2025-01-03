using MediatR;
using Npgsql;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.SubjectEntity.Commands.CreateSubject;

public class CreateSubjectCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateSubjectCommand, Subject>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

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

        return createdSubject;
    }
}
