using MediatR;
using ScheduleService.Application.Common.Exceptions.SpecialityTeacherSubjectEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.SpecialityTeacherSubjectEntity.Queries.GetSpecialityTeacherSubjectById;

public class GetSpecialityTeacherSubjectByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetSpecialityTeacherSubjectByIdQuery, SpecialityTeacherSubject>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<SpecialityTeacherSubject> Handle(
        GetSpecialityTeacherSubjectByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _unitOfWork.SpecialityTeacherSubjectRepository.GetByIdAsync(
            request.SpecialityId,
            request.Course,
            request.Subgroup
        );

        if (entity == null)
        {
            throw new SpecialityTeacherSubjectNotFoundException(
                request.SpecialityId,
                request.Course,
                request.Subgroup
            );
        }

        return entity;
    }
}
