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
        EnsureCreated("scheduleDb");
    }

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_configuration.GetConnectionString("DbConnectionString"));
    }

    public void EnsureCreated(string name)
    {
        var parameters = new DynamicParameters();

        parameters.Add("name", name);

        using var connection = CreateConnection();

        var databaseExists = connection.QueryFirstOrDefault<bool>(
            "SELECT EXISTS (SELECT 1 FROM pg_database WHERE datname = @name)"
        );

        if (!databaseExists)
        {
            connection.Execute("CREATE DATABASE @name", new { name });
        }
    }
}
