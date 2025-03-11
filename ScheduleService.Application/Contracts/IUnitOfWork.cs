namespace ScheduleService.Application.Contracts;

public interface IUnitOfWork
{
    IColorRepository ColorRepository { get; }
    IRoomRepository RoomRepository { get; }
    ISubjectRepository SubjectRepository { get; }
    IWeekdayRepository WeekdayRepository { get; }
    ICurrentWeekdayRepository CurrentWeekdayRepository { get; }
    IClassRepository ClassRepository { get; }
    ISpecialityTeacherSubjectRepository SpecialityTeacherSubjectRepository { get; }
    void CommitTransaction();
    void RollbackTransaction();
}
