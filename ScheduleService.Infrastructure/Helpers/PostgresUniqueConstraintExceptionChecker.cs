using System.Reflection;
using Npgsql;
using ScheduleService.Application.Contracts.Helpers;
using ScheduleService.Domain.Attributes;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Infrastructure.Helpers;

public class PostgresUniqueConstraintExceptionChecker : IUniqueConstraintExceptionChecker
{
    public string? Check<T>(Exception exception)
        where T : BaseModel
    {
        if (
            exception is PostgresException postgresException
            && postgresException.SqlState == PostgresErrorCodes.UniqueViolation
        )
        {
            var unAttrs = typeof(T)
                .GetProperties()
                .Select(prop => new
                {
                    property = prop.Name,
                    attribute = prop.GetCustomAttribute<UniqueAttribute>(),
                })
                .Where(prop => prop.attribute != null);

            var unAt = unAttrs.FirstOrDefault(prop =>
                postgresException.ConstraintName.Contains(prop.attribute.Name)
            );

            if (unAt != null)
            {
                return unAt.property;
            }
        }

        return null;
    }
}
