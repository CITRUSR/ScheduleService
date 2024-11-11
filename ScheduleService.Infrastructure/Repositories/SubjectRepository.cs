using System.Text;
using Dapper;
using ScheduleService.Application.Common.Models;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.SubjectEntity.Queries.GetSubjects;
using ScheduleService.Domain.Entities;
using ScheduleService.Infrastructure.Repositories.Sql;

namespace ScheduleService.Infrastructure.Repositories;

public class SubjectRepository(IDbContext dbContext) : ISubjectRepository
{
    private readonly IDbContext _dbContext = dbContext;

    public async Task<Subject?> DeleteAsync(int id)
    {
        using var connection = _dbContext.CreateConnection();

        var subject = await connection.QueryFirstOrDefaultAsync<Subject>(
            SubjectQueries.DeleteSubject,
            new { id }
        );

        return subject;
    }

    public async Task<List<Subject>> GetAsync(
        SubjectFilter filter,
        PaginationParameters paginationParameters
    )
    {
        using var connection = _dbContext.CreateConnection();

        var sqlBuilder = new StringBuilder();

        sqlBuilder.Append(SubjectQueries.GetSubjects);

        if (!string.IsNullOrWhiteSpace(filter.SearchString))
        {
            sqlBuilder.Append(
                $" WHERE LOWER(CONCAT(name, abbreviation)) LIKE LOWER('%{filter.SearchString}%')"
            );
        }

        string orderCol = filter.FilterBy switch
        {
            SubjectFilterState.Name => "name",
            SubjectFilterState.Abbreviation => "abbreviation",
        };

        sqlBuilder.Append($" ORDER BY {orderCol} {filter.OrderState}");

        sqlBuilder.Append($" LIMIT {paginationParameters.PageSize}");

        sqlBuilder.Append(
            $" OFFSET {(paginationParameters.Page - 1) * paginationParameters.PageSize}"
        );

        var subjects = await connection.QueryAsync<Subject>(sqlBuilder.ToString());

        return [.. subjects];
    }

    public async Task<Subject?> GetByIdAsync(int id)
    {
        using var connection = _dbContext.CreateConnection();

        var subject = await connection.QueryFirstOrDefaultAsync<Subject>(
            SubjectQueries.GetSubjectById,
            new { id }
        );

        return subject;
    }

    public async Task<Subject> InsertAsync(Subject subject)
    {
        using var connection = _dbContext.CreateConnection();

        var subjectId = await connection.QuerySingleAsync<int>(
            SubjectQueries.InsertSubject,
            new { subject.Name, subject.Abbreviation }
        );

        subject.Id = subjectId;

        return subject;
    }

    public async Task<Subject?> UpdateAsync(Subject subject)
    {
        using var connection = _dbContext.CreateConnection();

        var affectedRows = await connection.ExecuteAsync(
            SubjectQueries.UpdateSubject,
            new
            {
                subject.Name,
                subject.Abbreviation,
                subject.Id
            }
        );

        return affectedRows == 1 ? subject : null;
    }
}
