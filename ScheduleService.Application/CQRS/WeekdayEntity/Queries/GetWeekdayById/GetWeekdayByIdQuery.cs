using MediatR;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.WeekdayEntity.Queries.GetWeekdayById;

public record GetWeekdayByIdQuery(int Id) : IRequest<Weekday>;
