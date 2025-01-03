namespace ScheduleService.Application.Contracts;

public interface ICurrentWeekdayUpdateTaskCreator
{
    void UpdateCurrentWeekdayTask(string color, TimeSpan interval, DateTime updateTime);
}
