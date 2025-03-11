using MediatR;
using Npgsql;
using ScheduleService.Application.Common.Exceptions.SpecialityTeacherSubjectEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.Contracts.Services;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.SpecialityTeacherSubjectEntity.Commands.CreateSpecialityTeacherSubject;

public class CreateSpecialityTeacherSubjectCommandHandler(
    IUnitOfWork unitOfWork,
    ISpecialityTeacherSubjectRelatedDataChecker dataChecker
) : IRequestHandler<CreateSpecialityTeacherSubjectCommand, SpecialityTeacherSubject>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ISpecialityTeacherSubjectRelatedDataChecker _dataChecker = dataChecker;

    public async Task<SpecialityTeacherSubject> Handle(
        CreateSpecialityTeacherSubjectCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = new SpecialityTeacherSubject()
        {
            SpecialityId = request.SpecialityId,
            Course = request.Course,
            TeacherId = request.TeacherId,
            SubjectId = request.SubjectId,
            SubGroup = request.SubGroup,
        };

        await _dataChecker.Check(entity);

        try
        {
            await _unitOfWork.SpecialityTeacherSubjectRepository.InsertAsync(entity);

            _unitOfWork.CommitTransaction();
        }
        catch (Exception ex)
        {
            if (ex is PostgresException postgresException)
            {
                if (postgresException.SqlState == "23505")
                {
                    if (postgresException.Detail.Contains("speciality_fk"))
                    {
                        throw new PrimarySpecialityTeacherSubjectAlreadyExistsException(
                            request.SpecialityId,
                            request.Course,
                            request.SubGroup
                        );
                    }
                    else if (postgresException.Detail.Contains("speciality_fk"))
                    {
                        if (postgresException.Detail.Contains("teacher_fk"))
                        {
                            throw new TeacherSubjectCombinationAlreadyExistsException(
                                request.TeacherId,
                                request.SubjectId
                            );
                        }
                    }
                }
            }

            throw;
        }

        return entity;
    }
}
