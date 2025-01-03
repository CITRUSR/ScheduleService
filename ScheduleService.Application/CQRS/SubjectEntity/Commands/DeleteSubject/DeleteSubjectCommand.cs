using MediatR;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.SubjectEntity.Commands.DeleteSubject;

public record DeleteSubjectCommand(int Id) : IRequest<Subject>;
