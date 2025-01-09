using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.CQRS.ColorEntity.Responses;

namespace ScheduleService.Application.Common.Extensions;

public static class ColorExtension
{
    public static void EnsureColorExists(this IEnumerable<ColorViewModel> colors, string name)
    {
        if (!colors.Any(c => c.Name == name))
        {
            throw new ColorNotFoundException(name);
        }
    }
}
