using MediatR;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.SpecialityTeacherSubjectEntity.Queries.GetAllSpecialityTeacherSubjects;

public record GetAllSpecialityTeacherSubjectsQuery : IRequest<List<SpecialityTeacherSubject>>;
