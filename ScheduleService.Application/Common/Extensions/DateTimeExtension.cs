namespace ScheduleService.Application.Common.Extensions;

public static class DateTimeExtension
{
    public static int GetCurrentWeekdayOrder(this DateTime date)
    {
        return ((int)date.DayOfWeek + 6) % 7 + 1;
    }
}
