using MediatR;
using ScheduleService.Application.CQRS.ColorEntity.Responses;

namespace ScheduleService.Application.CQRS.ColorEntity.Queries.GetColors;

public record GetColorsQuery : IRequest<List<ColorViewModel>>;
