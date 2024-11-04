using System.Data;
using ScheduleService.Application.Contracts;

namespace ScheduleService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    public IColorRepository ColorRepository { get; }
    private readonly IDbContext _dbContext;

    private readonly IDbConnection _connection;
    private IDbTransaction _transaction;

    public UnitOfWork(IDbContext dbContext, IColorRepository colorRepository)
    {
        _dbContext = dbContext;
        _connection = _dbContext.CreateConnection();
        _connection.Open();
        _transaction = _connection.BeginTransaction();
        ColorRepository = colorRepository;
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