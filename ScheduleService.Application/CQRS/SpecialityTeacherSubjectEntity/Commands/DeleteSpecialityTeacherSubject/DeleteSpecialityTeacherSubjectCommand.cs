using MediatR;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.SpecialityTeacherSubjectEntity.Commands.DeleteSpecialityTeacherSubject;

public record DeleteSpecialityTeacherSubjectCommand(int SpecialityId, int Course, int Subgroup)
    : IRequest<SpecialityTeacherSubject>;
