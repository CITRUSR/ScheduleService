using MediatR;
using ScheduleService.Application.Common.Exceptions.SpecialityTeacherSubjectEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.SpecialityTeacherSubjectEntity.Commands.DeleteSpecialityTeacherSubject;

public class DeleteSpecialityTeacherSubjectCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteSpecialityTeacherSubjectCommand, SpecialityTeacherSubject>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<SpecialityTeacherSubject> Handle(
        DeleteSpecialityTeacherSubjectCommand request,
        CancellationToken cancellationToken
    )
    {
        var deletedEntity = await _unitOfWork.SpecialityTeacherSubjectRepository.DeleteAsync(
            request.SpecialityId,
            request.Course,
            request.Subgroup
        );

        if (deletedEntity == null)
        {
            throw new SpecialityTeacherSubjectNotFoundException(
                request.SpecialityId,
                request.Course,
                request.Subgroup
            );
        }

        _unitOfWork.CommitTransaction();

        return deletedEntity;
    }
}
