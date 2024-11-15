using MediatR;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.CurrentWeekdayEntity.Commands.UpdateCurrentWeekday;

public record UpdateCurrentWeekdayCommand(string Color, TimeSpan Interval, DateTime UpdateTime)
    : IRequest<CurrentWeekday>;
