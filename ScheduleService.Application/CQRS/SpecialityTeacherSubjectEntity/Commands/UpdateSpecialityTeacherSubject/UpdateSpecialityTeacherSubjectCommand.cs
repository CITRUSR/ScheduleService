using MediatR;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.SpecialityTeacherSubjectEntity.Commands.UpdateSpecialityTeacherSubject;

public record UpdateSpecialityTeacherSubjectCommand(
    int SpecialityId,
    int Course,
    int Subgroup,
    Guid TeacherId,
    int SubjectId
) : IRequest<SpecialityTeacherSubject>;
