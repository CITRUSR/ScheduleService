using MediatR;
using ScheduleService.Application.Common.Constants;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.CurrentWeekdayEntity.Commands.UpdateCurrentWeekday;
using ScheduleService.Application.CQRS.CurrentWeekdayEntity.Queries.GetCurrentWeekday;

namespace ScheduleService.Application.Services;

public class CurrentWeekdayUpdateTaskCreator(
    IScheduleService scheduleService,
    IMediator mediator,
    IUnitOfWork unitOfWork
) : ICurrentWeekdayUpdateTaskCreator
{
    private readonly IScheduleService _scheduleService = scheduleService;
    private readonly IMediator _mediator = mediator;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public void UpdateCurrentWeekdayTask(string color, TimeSpan interval, DateTime updateTime)
    {
        _scheduleService.DoIn(
            () => CurrentWeekdayUpdateRecure(color, interval, updateTime),
            updateTime
        );
    }

    public void CurrentWeekdayUpdateRecure(string color, TimeSpan interval, DateTime updateTime)
    {
        _scheduleService.RecureAction(
            ScheduleKeys.UpdateCurrentWeekdayTaskId,
            () => UpdateCurrentWeekday(color, interval, updateTime),
            $"0 0 */{interval.Days} * *"
        );
    }

    public async Task UpdateCurrentWeekday(string color, TimeSpan interval, DateTime updateTime)
    {
        var CurrentWeekday = await _mediator.Send(new GetCurrentWeekdayQuery());

        var colors = await _unitOfWork.ColorRepository.GetAllAsync();

        var colorIndex = colors.FindIndex(x => x.Name == CurrentWeekday.Color);

        color = colorIndex < colors.Count - 1 ? colors[colorIndex + 1].Name : colors[0].Name;

        var command = new UpdateCurrentWeekdayCommand(color, interval, updateTime);

        await _mediator.Send(command);
    }
}
