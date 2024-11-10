using MediatR;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.SubjectEntity.Commands.CreateSubject;

public record CreateSubjectCommand(string Name, string? Abbreviation) : IRequest<Subject>;
