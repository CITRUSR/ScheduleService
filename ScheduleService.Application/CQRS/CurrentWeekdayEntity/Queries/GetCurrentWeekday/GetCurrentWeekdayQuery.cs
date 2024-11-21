using MediatR;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.CurrentWeekdayEntity.Queries.GetCurrentWeekday;

public record GetCurrentWeekdayQuery() : IRequest<CurrentWeekday>;
