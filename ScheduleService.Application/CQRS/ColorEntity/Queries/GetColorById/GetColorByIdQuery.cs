using MediatR;
using ScheduleService.Application.CQRS.ColorEntity.Responses;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.ColorEntity.Queries.GetColorById;

public record GetColorByIdQuery(int Id) : IRequest<ColorViewModel>;
