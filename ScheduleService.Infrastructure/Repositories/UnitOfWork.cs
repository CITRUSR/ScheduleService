using System.Data;
using ScheduleService.Application.Contracts;

namespace ScheduleService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    public IColorRepository ColorRepository { get; }
    public IRoomRepository RoomRepository { get; }
    public ISubjectRepository SubjectRepository { get; }
    public IWeekdayRepository WeekdayRepository { get; }
    public ICurrentWeekdayRepository CurrentWeekdayRepository { get; }

    public IClassRepository ClassRepository { get; }

    private readonly IDbContext _dbContext;

    private readonly IDbConnection _connection;
    private IDbTransaction _transaction;

    public UnitOfWork(
        IDbContext dbContext,
        IColorRepository colorRepository,
        IRoomRepository roomRepository,
        ISubjectRepository subjectRepository,
        IWeekdayRepository weekdayRepository,
        ICurrentWeekdayRepository currentWeekdayRepository,
        IClassRepository classRepository
    )
    {
        _dbContext = dbContext;
        _connection = _dbContext.CreateConnection();
        _connection.Open();
        _transaction = _connection.BeginTransaction();
        ColorRepository = colorRepository;
        RoomRepository = roomRepository;
        SubjectRepository = subjectRepository;
        WeekdayRepository = weekdayRepository;
        CurrentWeekdayRepository = currentWeekdayRepository;
        ClassRepository = classRepository;
    }

    public void CommitTransaction()
    {
        _transaction.Commit();
        _transaction.Dispose();
        _transaction = _connection.BeginTransaction();
    }

    public void RollbackTransaction()
    {
        _transaction.Rollback();
    }

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }
}
