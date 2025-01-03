using ScheduleService.Application.Common.Models;

namespace ScheduleService.Application.CQRS.SubjectEntity.Queries.GetSubjects;

public class SubjectFilter
{
    public string? SearchString { get; set; }
    public SubjectFilterState FilterBy { get; set; }
    public OrderState OrderState { get; set; }
}
