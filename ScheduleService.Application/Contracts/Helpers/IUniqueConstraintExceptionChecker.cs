using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.Contracts.Helpers;

public interface IUniqueConstraintExceptionChecker
{
    string? Check<T>(Exception exception)
        where T : BaseModel;
}
