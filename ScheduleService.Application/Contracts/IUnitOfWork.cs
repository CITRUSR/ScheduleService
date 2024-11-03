namespace ScheduleService.Application.Contracts;

public interface IUnitOfWork
{
    void CommitTransaction();
    void RollbackTransaction();
}
