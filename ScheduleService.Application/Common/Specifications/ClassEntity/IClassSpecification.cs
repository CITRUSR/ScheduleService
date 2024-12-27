using ScheduleService.Application.Contracts.Specifications;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.Common.Specifications.ClassEntity;

public interface IClassSpecification : ISpecification<Class>
{
    DateTime LeftChangeDateLimiter { get; }
    DateTime RightChangeDateLimiter { get; }
}
