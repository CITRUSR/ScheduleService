using MediatR;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.SubjectEntity.Queries.GetSubjectById;

public record GetSubjectByIdQuery(int Id) : IRequest<Subject>;
