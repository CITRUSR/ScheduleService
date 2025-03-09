using MediatR;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.SpecialityTeacherSubjectEntity.Queries.GetAllSpecialityTeacherSubjects;

public class GetAllSpecialityTeacherSubjectsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllSpecialityTeacherSubjectsQuery, List<SpecialityTeacherSubject>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<List<SpecialityTeacherSubject>> Handle(
        GetAllSpecialityTeacherSubjectsQuery request,
        CancellationToken cancellationToken
    )
    {
        var entities = await _unitOfWork.SpecialityTeacherSubjectRepository.GetAllAsync();

        return entities;
    }
}
