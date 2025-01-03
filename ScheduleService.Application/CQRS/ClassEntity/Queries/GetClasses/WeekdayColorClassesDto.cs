using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses;

public class WeekdayColorClassesDto<TColorClasses, KClassDetail>
    where TColorClasses : ColorClassesDto<KClassDetail>
    where KClassDetail : ClassDetailBase
{
    public Weekday Weekday { get; set; }
    public List<TColorClasses> Classes { get; set; }
}
