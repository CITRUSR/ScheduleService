using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses;

public class ColorClassesDto<TClassDetail>
    where TClassDetail : ClassDetailBase
{
    public Color? Color { get; set; }
    public List<TClassDetail> Classes { get; set; } = [];
}
