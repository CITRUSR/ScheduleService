using MediatR;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClassById;

public record GetClassByIdQuery(int Id) : IRequest<Class>;
