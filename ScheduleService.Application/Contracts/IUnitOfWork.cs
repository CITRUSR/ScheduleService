namespace ScheduleService.Application.Contracts;

public interface IUnitOfWork
{
    IColorRepository ColorRepository { get; }
    void CommitTransaction();
    void RollbackTransaction();
}
