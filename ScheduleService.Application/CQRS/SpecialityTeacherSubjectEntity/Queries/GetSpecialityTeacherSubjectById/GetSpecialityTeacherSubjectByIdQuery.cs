using MediatR;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.SpecialityTeacherSubjectEntity.Queries.GetSpecialityTeacherSubjectById;

public record GetSpecialityTeacherSubjectByIdQuery(int SpecialityId, int Course, int Subgroup)
    : IRequest<SpecialityTeacherSubject>;
