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
}
