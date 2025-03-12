using MediatR;
using ScheduleService.Application.Common.Exceptions.SpecialityTeacherSubjectEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.Contracts.Helpers;
using ScheduleService.Application.Contracts.Services;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.SpecialityTeacherSubjectEntity.Commands.UpdateSpecialityTeacherSubject;

public class UpdateSpecialityTeacherSubjectCommandHandler(
    IUnitOfWork unitOfWork,
    ISpecialityTeacherSubjectRelatedDataChecker spTcSbRelatedDataChecker,
    IUniqueConstraintExceptionChecker uniqueChecker
) : IRequestHandler<UpdateSpecialityTeacherSubjectCommand, SpecialityTeacherSubject>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ISpecialityTeacherSubjectRelatedDataChecker _spTcSbRelatedDataChecker =
        spTcSbRelatedDataChecker;
    private readonly IUniqueConstraintExceptionChecker _uniqueChecker = uniqueChecker;
    private readonly HashSet<string> pKeyFields = ["speciality", "course", "subgroup"];
    private readonly HashSet<string> tchSubFields = ["teacher", "subject"];

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
            var field = _uniqueChecker.Check<SpecialityTeacherSubject>(ex);

            if (field != null)
            {
                if (pKeyFields.Contains(field, StringComparer.OrdinalIgnoreCase))
                {
                    throw new PrimarySpecialityTeacherSubjectAlreadyExistsException(
                        updatedSpecialityTeacherSubject.SpecialityId,
                        updatedSpecialityTeacherSubject.Course,
                        updatedSpecialityTeacherSubject.SubGroup
                    );
                }
                else if (tchSubFields.Contains(field, StringComparer.OrdinalIgnoreCase))
                {
                    throw new TeacherSubjectCombinationAlreadyExistsException(
                        updatedSpecialityTeacherSubject.TeacherId,
                        updatedSpecialityTeacherSubject.SubjectId
                    );
                }
            }

            throw;
        }

        return updatedSpecialityTeacherSubject;
    }
}
