using Dapper;
using Newtonsoft.Json;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.ClassEntity;
using ScheduleService.Application.CQRS.ClassEntity.Commands.CreateClass;
using ScheduleService.Application.CQRS.ClassEntity.Commands.UpdateClass;
using ScheduleService.Domain.Entities;
using ScheduleService.Infrastructure.Repositories.Sql;

namespace ScheduleService.Infrastructure.Repositories;

public class ClassRepository(IDbContext dbContext) : IClassRepository
{
    private readonly IDbContext _dbContext = dbContext;

    public Task<Class?> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Class>> GetAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Class?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Class> InsertAsync(CreateClassDto dto)
    {
        using var connection = _dbContext.CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("GroupFk", dto.GroupId);
        parameters.Add("SubjectFk", dto.SubjectId);
        parameters.Add("WeekdayFk", dto.WeekdayId);
        parameters.Add("ColorFk", dto.ColorId);
        parameters.Add("StartsAt", dto.StartsAt);
        parameters.Add("EndsAt", dto.EndsAt);
        parameters.Add("ChangeOn", dto.ChangeOn);

        var classes = await connection.QueryAsync<Class, Subject, Weekday, string, string, Class>(
            string.Format(
                ClassQueries.InsertClass,
                string.Join(", ", dto.TeacherIds.Select(guid => $"'{guid}'::uuid")),
                string.Join(", ", dto.RoomIds)
            ),
            (@cl, subject, weekday, teacherIds, rooms) =>
            {
                @cl.Subject = subject;
                @cl.Weekday = weekday;
                @cl.TeacherIds = JsonConvert.DeserializeObject<List<Guid>>(teacherIds);
                @cl.Rooms = JsonConvert.DeserializeObject<List<Room>>(rooms);
                return @cl;
            },
            parameters,
            splitOn: "subjectId, weekdayId, teachers, rooms"
        );

        var @class = classes.FirstOrDefault();

        return @class;
    }

    public async Task<ClassDependenciesDto> GetClassDependencies(GetClassDependenciesDto dto)
    {
        using var connection = _dbContext.CreateConnection();

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("ColorId", dto.ColorId);
        parameters.Add("SubjectId", dto.SubjectId);
        parameters.Add("WeekdayId", dto.WeekdayId);

        using var multy = await connection.QueryMultipleAsync(
            $@"
            {(dto.ColorId != null ? ColorQueries.GetColorById : "")};
            {SubjectQueries.GetSubjectById};
            {WeekdayQueries.GetWeekdayById};
            {string.Format(RoomQueries.GetRoomsById, string.Join(",", dto.RoomIds))};",
            parameters
        );

        var colorTask = dto.ColorId != null ? await multy.ReadFirstOrDefaultAsync<Color>() : null;

        var subjectTask = await multy.ReadFirstOrDefaultAsync<Subject>();

        var weekdayTask = await multy.ReadFirstOrDefaultAsync<Weekday>();

        var roomsCountTask = await multy.ReadAsync<Room>();

        return new ClassDependenciesDto(colorTask, subjectTask, weekdayTask, [.. roomsCountTask]);
    }

    public async Task<Class?> UpdateAsync(UpdateClassDto dto)
    {
        using var connection = _dbContext.CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("ClassId", dto.Id);
        parameters.Add("GroupFk", dto.GroupId);
        parameters.Add("ColorFk", dto.ColorId);
        parameters.Add("SubjectFK", dto.SubjectId);
        parameters.Add("WeekdayFk", dto.WeekdayId);
        parameters.Add("StartsAt", dto.StartsAt);
        parameters.Add("EndsAt", dto.EndsAt);
        parameters.Add("ChangeOn", dto.ChangeOn);

        var classes = await connection.QueryAsync<
            Class,
            Subject,
            Weekday,
            Color?,
            string,
            string,
            Class
        >(
            string.Format(
                ClassQueries.UpdateClass,
                string.Join(", ", dto.TeacherIds.Select(x => $"'{x}'::uuid")),
                string.Join(", ", dto.RoomIds)
            ),
            (@cl, subject, weekday, color, teacherIds, rooms) =>
            {
                @cl.Subject = subject;
                @cl.Weekday = weekday;
                @cl.Color = color;
                @cl.TeacherIds = JsonConvert.DeserializeObject<List<Guid>>(teacherIds);
                @cl.Rooms = JsonConvert.DeserializeObject<List<Room>>(rooms);

                return @cl;
            },
            parameters,
            splitOn: "id, id, id, teachersids, rooms"
        );

        var @class = classes.FirstOrDefault();

        return @class;
    }
}
