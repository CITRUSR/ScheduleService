using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses;

namespace ScheduleService.Application.Common.Extensions;

public static class ColorClassedDtoExtension
{
    public static void CountClassOrder<T>(this List<ColorClassesDto<T>> dto)
        where T : ClassDetailBase
    {
        foreach (var colorClass in dto)
        {
            for (var i = 0; i < colorClass.Classes.Count; i++)
            {
                colorClass.Classes[i].Order = i + 1;
            }
        }
    }
}
