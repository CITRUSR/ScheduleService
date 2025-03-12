using MediatR;
using ScheduleService.Application.Common.Exceptions.SpecialityTeacherSubjectEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.Contracts.Helpers;
using ScheduleService.Application.Contracts.Services;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.SpecialityTeacherSubjectEntity.Commands.CreateSpecialityTeacherSubject;

public class CreateSpecialityTeacherSubjectCommandHandler(
    IUnitOfWork unitOfWork,
    ISpecialityTeacherSubjectRelatedDataChecker dataChecker,
    IUniqueConstraintExceptionChecker uniqueChecker
) : IRequestHandler<CreateSpecialityTeacherSubjectCommand, SpecialityTeacherSubject>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ISpecialityTeacherSubjectRelatedDataChecker _dataChecker = dataChecker;
    private readonly IUniqueConstraintExceptionChecker _uniqueChecker = uniqueChecker;
    private readonly HashSet<string> pKeyFields = ["speciality", "course", "subgroup"];
    private readonly HashSet<string> tchSubFields = ["teacher", "subject"];

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
            var field = _uniqueChecker.Check<SpecialityTeacherSubject>(ex);

            if (field != null)
            {
                if (pKeyFields.Contains(field, StringComparer.OrdinalIgnoreCase))
                {
                    throw new PrimarySpecialityTeacherSubjectAlreadyExistsException(
                        request.SpecialityId,
                        request.Course,
                        request.SubGroup
                    );
                }
                else if (tchSubFields.Contains(field, StringComparer.OrdinalIgnoreCase))
                {
                    throw new TeacherSubjectCombinationAlreadyExistsException(
                        request.TeacherId,
                        request.SubjectId
                    );
                }
            }

            throw;
        }

        return entity;
    }
}
