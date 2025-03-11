using MediatR;
using Npgsql;
using ScheduleService.Application.Common.Exceptions.SpecialityTeacherSubjectEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.Contracts.Services;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.SpecialityTeacherSubjectEntity.Commands.UpdateSpecialityTeacherSubject;

public class UpdateSpecialityTeacherSubjectCommandHandler(
    IUnitOfWork unitOfWork,
    ISpecialityTeacherSubjectRelatedDataChecker spTcSbRelatedDataChecker
) : IRequestHandler<UpdateSpecialityTeacherSubjectCommand, SpecialityTeacherSubject>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ISpecialityTeacherSubjectRelatedDataChecker _spTcSbRelatedDataChecker =
        spTcSbRelatedDataChecker;

    public async Task<SpecialityTeacherSubject> Handle(
        UpdateSpecialityTeacherSubjectCommand request,
        CancellationToken cancellationToken
    )
    {
        var updatedSpecialityTeacherSubject = new SpecialityTeacherSubject()
        {
            Course = request.Course,
            SpecialityId = request.SpecialityId,
            SubGroup = request.Subgroup,
            SubjectId = request.SubjectId,
            TeacherId = request.TeacherId,
        };

        try
        {
            await _spTcSbRelatedDataChecker.Check(updatedSpecialityTeacherSubject);

            var result = await _unitOfWork.SpecialityTeacherSubjectRepository.UpdateAsync(
                updatedSpecialityTeacherSubject
            );

            if (result == null)
            {
                throw new SpecialityTeacherSubjectNotFoundException(
                    request.SpecialityId,
                    request.Course,
                    request.Subgroup
                );
            }

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
                            updatedSpecialityTeacherSubject.SpecialityId,
                            updatedSpecialityTeacherSubject.Course,
                            updatedSpecialityTeacherSubject.SubGroup
                        );
                    }
                    else if (postgresException.Detail.Contains("speciality_fk"))
                    {
                        if (postgresException.Detail.Contains("teacher_fk"))
                        {
                            throw new TeacherSubjectCombinationAlreadyExistsException(
                                updatedSpecialityTeacherSubject.TeacherId,
                                updatedSpecialityTeacherSubject.SubjectId
                            );
                        }
                    }
                }
            }

            throw;
        }

        return updatedSpecialityTeacherSubject;
    }
}
