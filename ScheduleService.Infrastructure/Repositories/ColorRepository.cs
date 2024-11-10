using Dapper;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;
using ScheduleService.Infrastructure.Repositories.Sql;

namespace ScheduleService.Infrastructure.Repositories;

public class ColorRepository(IDbContext dbContext) : IColorRepository
{
    private readonly IDbContext _dbContext = dbContext;

    public async Task DeleteAsync(int id)
    {
        using var connection = _dbContext.CreateConnection();

        await connection.ExecuteAsync(ColorQueries.DeleteColor, new { id });
    }

    public async Task<List<Color>> GetAllAsync()
    {
        using var connection = _dbContext.CreateConnection();

        var colors = await connection.QueryAsync<Color>(ColorQueries.GetAllColors);

        return [.. colors];
    }

    public async Task<Color?> GetByIdAsync(int id)
    {
        using var connection = _dbContext.CreateConnection();
        var color = await connection.QueryFirstOrDefaultAsync<Color>(
            ColorQueries.GetColorById,
            new { id }
        );
        return color;
    }

    public async Task<Color> InsertAsync(Color color)
    {
        using var connection = _dbContext.CreateConnection();
        var colorId = await connection.QuerySingleAsync<int>(
            ColorQueries.InsertColor,
            new { color.Name }
        );

        var createdColor = new Color { Id = colorId, Name = color.Name };

        return createdColor;
    }

    public async Task<Color?> UpdateAsync(Color color)
    {
        using var connection = _dbContext.CreateConnection();

        var affectedRows = await connection.ExecuteAsync(
            ColorQueries.UpdateColor,
            new { color.Name, color.Id }
        );

        return affectedRows == 1 ? color : null;
    }
}
