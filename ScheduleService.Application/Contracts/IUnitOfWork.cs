namespace ScheduleService.Application.Contracts;

public interface IUnitOfWork
{
    IColorRepository ColorRepository { get; }
    IRoomRepository RoomRepository { get; }
    ISubjectRepository SubjectRepository { get; }
    void CommitTransaction();
    void RollbackTransaction();
}
