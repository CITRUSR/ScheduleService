using System.Data;
using System.Text;
using Dapper;
using ScheduleService.Application.Common.Models;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.SubjectEntity.Queries.GetSubjects;
using ScheduleService.Domain.Entities;
using ScheduleService.Infrastructure.Repositories.Sql;

namespace ScheduleService.Infrastructure.Repositories;

public class SubjectRepository(IDbConnection dbConnection) : ISubjectRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task<Subject?> DeleteAsync(int id)
    {
        var subject = await _dbConnection.QueryFirstOrDefaultAsync<Subject>(
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

        using var multy = await _dbConnection.QueryMultipleAsync(
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
        var parameters = new DynamicParameters();

        parameters.Add("SubjectId", id);

        var subject = await _dbConnection.QueryFirstOrDefaultAsync<Subject>(
            SubjectQueries.GetSubjectById,
            parameters
        );

        return subject;
    }

    public async Task<Subject> InsertAsync(Subject subject)
    {
        var subjectId = await _dbConnection.QuerySingleAsync<int>(
            SubjectQueries.InsertSubject,
            new { subject.Name, subject.Abbreviation }
        );

        subject.Id = subjectId;

        return subject;
    }

    public async Task<Subject?> UpdateAsync(Subject subject)
    {
        var affectedRows = await _dbConnection.ExecuteAsync(
            SubjectQueries.UpdateSubject,
            new
            {
                subject.Name,
                subject.Abbreviation,
                subject.Id,
            }
        );

        return affectedRows == 1 ? subject : null;
    }
}
