using Mapster;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.Common.Extensions;

public static class ClassExtension
{
    public static List<ColorClassesDto<TClassDetail>> ToColorClasses<TClassDetail>(
        this List<Class> classes
    )
        where TClassDetail : ClassDetailBase
    {
        var colorClasses = classes
            .GroupBy(x => x.Color)
            .Select(x => new ColorClassesDto<TClassDetail>
            {
                Color = x.Key,
                Classes = [.. x.Select(x => x.Adapt<TClassDetail>())]
            })
            .ToList();

        return colorClasses;
    }

    public static List<
        WeekdayColorClassesDto<TColorClassesDto, KClassDetail>
    > ToWeekdayColorClasses<TColorClassesDto, KClassDetail>(
        this List<Class> classes,
        List<Weekday> weekdays
    )
        where TColorClassesDto : ColorClassesDto<KClassDetail>
        where KClassDetail : ClassDetailBase
    {
        var weekdayClasses = classes
            .GroupBy(x => x.Weekday.Id)
            .Select(x => new WeekdayColorClassesDto<TColorClassesDto, KClassDetail>()
            {
                Weekday = weekdays[x.Key - 1],
                Classes = [.. x.ToList().ToColorClasses<KClassDetail>().Cast<TColorClassesDto>()]
            })
            .ToList();

        return weekdayClasses;
    }
}
