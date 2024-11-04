using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using ScheduleService.Application.Contracts;

namespace ScheduleService.Infrastructure;

public class DbContext : IDbContext
{
    private readonly IConfiguration _configuration;

    public DbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_configuration.GetConnectionString("DbConnectionString"));
    }
}
