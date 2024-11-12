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

    public async Task<PagedList<Subject>> GetAsync(
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
                $" WHERE LOWER(CONCAT(name, abbreviation)) LIKE LOWER(@SearchString)"
            );
        }

        var searchString = $"%{filter.SearchString}%";

        var countQuery = sqlBuilder
            .ToString()
            .Replace(SubjectQueries.GetSubjects, "SELECT COUNT(*) FROM subjects");

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

        using var multy = await connection.QueryMultipleAsync(
            $"{countQuery}; {sqlBuilder};",
            new { searchString }
        );

        var totalCountTask = multy.ReadSingleOrDefaultAsync<int>();
        var subjectsTask = multy.ReadAsync<Subject>();

        await Task.WhenAll(totalCountTask, subjectsTask);

        return new PagedList<Subject>(
            [.. await subjectsTask],
            await totalCountTask,
            paginationParameters.Page,
            paginationParameters.PageSize
        );
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
