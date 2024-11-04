using Dapper;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;
using ScheduleService.Infrastructure.Repositories.Sql;

namespace ScheduleService.Infrastructure.Repositories;

public class ColorRepository(IDbContext dbContext) : IColorRepository
{
    private readonly IDbContext _dbContext = dbContext;

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Color>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Color> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
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

    public Task<Color> UpdateAsync(Color color)
    {
        throw new NotImplementedException();
    }
}
