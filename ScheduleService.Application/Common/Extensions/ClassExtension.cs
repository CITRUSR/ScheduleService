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
            .GroupBy(x => new { x.Color.Name, x.Color.Id })
            .Select(x => new ColorClassesDto<TClassDetail>
            {
                Color = new Color() { Id = x.Key.Id, Name = x.Key.Name },
                Classes = [.. x.Select(x => x.Adapt<TClassDetail>())]
            })
            .ToList();

        return colorClasses;
    }

    public static List<
        WeekdayColorClassesDto<TColorClassesDto, KClassDetail>
    > ToWeekdayColorClasses<TColorClassesDto, KClassDetail>(this List<Class> classes)
        where TColorClassesDto : ColorClassesDto<KClassDetail>
        where KClassDetail : ClassDetailBase
    {
        var weekdayClasses = classes
            .GroupBy(x => new { x.Weekday.Id, x.Weekday.Name })
            .Select(x => new WeekdayColorClassesDto<TColorClassesDto, KClassDetail>()
            {
                Weekday = new Weekday() { Id = x.Key.Id, Name = x.Key.Name },
                Classes = [.. x.ToList().ToColorClasses<KClassDetail>().Cast<TColorClassesDto>()]
            })
            .ToList();

        return weekdayClasses;
    }
}
