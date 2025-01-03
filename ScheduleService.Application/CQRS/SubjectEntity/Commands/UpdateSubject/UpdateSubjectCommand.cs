using MediatR;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.SubjectEntity.Commands.UpdateSubject;

public record UpdateSubjectCommand(int Id, string Name, string? Abbreviation) : IRequest<Subject>;
