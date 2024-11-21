using MediatR;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.CurrentWeekdayEntity.Commands.CreateCurrentWeekday;

public record CreateCurrentWeekdayCommand(string Color, TimeSpan Interval, DateTime UpdateTime)
    : IRequest<CurrentWeekday>;
