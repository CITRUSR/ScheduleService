using System.Data;
using Dapper;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;
using ScheduleService.Infrastructure.Repositories.Sql;

namespace ScheduleService.Infrastructure.Repositories;

public class ColorRepository(IDbConnection dbConnection) : IColorRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task<Color?> DeleteAsync(int id)
    {
        var color = await _dbConnection.QueryFirstOrDefaultAsync<Color>(
            ColorQueries.DeleteColor,
            new { id }
        );

        return color;
    }

    public async Task<List<Color>> GetAllAsync()
    {
        var colors = await _dbConnection.QueryAsync<Color>(ColorQueries.GetAllColors);

        return [.. colors];
    }

    public async Task<Color?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("ColorId", id);

        var color = await _dbConnection.QueryFirstOrDefaultAsync<Color>(
            ColorQueries.GetColorById,
            parameters
        );
        return color;
    }

    public async Task<Color> InsertAsync(Color color)
    {
        var colorId = await _dbConnection.QuerySingleAsync<int>(
            ColorQueries.InsertColor,
            new { color.Name }
        );

        var createdColor = new Color { Id = colorId, Name = color.Name };

        return createdColor;
    }

    public async Task<Color?> UpdateAsync(Color color)
    {
        var affectedRows = await _dbConnection.ExecuteAsync(
            ColorQueries.UpdateColor,
            new { color.Name, color.Id }
        );

        return affectedRows == 1 ? color : null;
    }
}
