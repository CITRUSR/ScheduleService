using System.Data;

namespace ScheduleService.Application.Contracts;

public interface IDbContext
{
    IDbConnection CreateConnection();
}
