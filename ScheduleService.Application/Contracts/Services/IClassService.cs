using ScheduleService.Application.Common.Specifications.ClassEntity;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.Contracts.Services;

public interface IClassService
{
    public Task<(
        List<ColorClassesDto<TClassDetail>>,
        Weekday weekday
    )> GetClassesForDay<TClassDetail>(IClassSpecification specification, int weekdayOrder)
        where TClassDetail : ClassDetailBase;

    public Task<
        List<WeekdayColorClassesDto<ColorClassesDto<TClassDetail>, TClassDetail>>
    > GetClassesForWeek<TClassDetail>(IClassSpecification specification)
        where TClassDetail : ClassDetailBase;
}
