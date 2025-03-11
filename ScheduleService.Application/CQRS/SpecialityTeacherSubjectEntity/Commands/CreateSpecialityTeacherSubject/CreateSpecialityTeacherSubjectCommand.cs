using MediatR;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.SpecialityTeacherSubjectEntity.Commands.CreateSpecialityTeacherSubject;

public record CreateSpecialityTeacherSubjectCommand(
    int SpecialityId,
    int Course,
    int SubGroup,
    Guid TeacherId,
    int SubjectId
) : IRequest<SpecialityTeacherSubject>;
