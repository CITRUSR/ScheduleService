using MediatR;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.WeekdayEntity.Queries.GetWeekdays;

public record GetWeekdaysQuery() : IRequest<List<Weekday>>;
