using System.Data;
using Dapper;
using Newtonsoft.Json;
using ScheduleService.Application.Common.Specifications.ClassEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.ClassEntity;
using ScheduleService.Application.CQRS.ClassEntity.Commands.CreateClass;
using ScheduleService.Application.CQRS.ClassEntity.Commands.UpdateClass;
using ScheduleService.Domain.Entities;
using ScheduleService.Infrastructure.Repositories.Sql;

namespace ScheduleService.Infrastructure.Repositories;

public class ClassRepository(IDbConnection dbConnection) : IClassRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task<Class?> DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();

        parameters.Add("ClassId", id);

        var classes = await _dbConnection.QueryAsync<
            Class,
            Subject,
            Weekday,
            Color?,
            string,
            string,
            Class
        >(
            ClassQueries.DeleteClass,
            (cl, subject, weekday, color, teacherIds, rooms) =>
            {
                cl.Subject = subject;
                cl.Weekday = weekday;
                cl.Color = color;
                cl.TeacherIds = JsonConvert.DeserializeObject<List<Guid>>(teacherIds);
                cl.Rooms = JsonConvert.DeserializeObject<List<Room>>(rooms);
                return cl;
            },
            parameters,
            splitOn: "id, id, id, teachersIds, rooms"
        );

        var @class = classes.FirstOrDefault();

        return @class;
    }

    public async Task<List<Class>> GetAsync(IClassSpecification specification)
    {
        var parameters = new DynamicParameters();

        parameters.Add("LeftChangeDateLimiter", specification.LeftChangeDateLimiter);
        parameters.Add("RightChangeDateLimiter", specification.RightChangeDateLimiter);

        var classes = await _dbConnection.QueryAsync<
            Class,
            Weekday,
            Subject,
            Color?,
            string,
            string,
            Class
        >(
            string.Format(ClassQueries.GetClasses, specification.WhereClause),
            (@class, weekday, subject, color, teacherIds, rooms) =>
            {
                @class.Subject = subject;
                @class.Color = color;
                @class.Weekday = weekday;
                @class.TeacherIds = JsonConvert.DeserializeObject<List<Guid>>(teacherIds);
                @class.Rooms = JsonConvert.DeserializeObject<List<Room>>(rooms);
                return @class;
            },
            parameters,
            splitOn: "id, id, id, teacherIds, rooms"
        );

        return [.. classes];
    }

    public async Task<Class?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();

        parameters.Add("ClassId", id);

        var classes = await _dbConnection.QueryAsync<
            Class,
            Subject,
            Weekday,
            Color?,
            string,
            string,
            Class
        >(
            ClassQueries.GetClassById,
            (cl, subject, weekday, color, teacherIds, rooms) =>
            {
                cl.Subject = subject;
                cl.Weekday = weekday;
                cl.Color = color;
                cl.TeacherIds = JsonConvert.DeserializeObject<List<Guid>>(teacherIds);
                cl.Rooms = JsonConvert.DeserializeObject<List<Room>>(rooms);
                return cl;
            },
            parameters,
            splitOn: "id, id, id, TeachersIds, rooms"
        );

        var @class = classes.FirstOrDefault();

        return @class;
    }

    public async Task<Class> InsertAsync(CreateClassDto dto)
    {
        var parameters = new DynamicParameters();
        parameters.Add("GroupFk", dto.GroupId);
        parameters.Add("SubjectFk", dto.SubjectId);
        parameters.Add("WeekdayFk", dto.WeekdayId);
        parameters.Add("ColorFk", dto.ColorId);
        parameters.Add("StartsAt", dto.StartsAt);
        parameters.Add("EndsAt", dto.EndsAt);
        parameters.Add("ChangeOn", dto.ChangeOn);

        var classes = await _dbConnection.QueryAsync<
            Class,
            Color?,
            Subject,
            Weekday,
            string,
            string,
            Class
        >(
            string.Format(
                ClassQueries.InsertClass,
                string.Join(", ", dto.TeacherIds.Select(guid => $"'{guid}'::uuid")),
                string.Join(", ", dto.RoomIds)
            ),
            (@cl, color, subject, weekday, teacherIds, rooms) =>
            {
                @cl.Subject = subject;
                @cl.Weekday = weekday;
                @cl.Color = color;
                @cl.TeacherIds = JsonConvert.DeserializeObject<List<Guid>>(teacherIds);
                @cl.Rooms = JsonConvert.DeserializeObject<List<Room>>(rooms);
                return @cl;
            },
            parameters,
            splitOn: "id, subjectId, weekdayId, teachers, rooms"
        );

        var @class = classes.FirstOrDefault();

        return @class;
    }

    public async Task<ClassDependenciesDto> GetClassDependencies(GetClassDependenciesDto dto)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("ColorId", dto.ColorId);
        parameters.Add("SubjectId", dto.SubjectId);
        parameters.Add("WeekdayId", dto.WeekdayId);

        using var multy = await _dbConnection.QueryMultipleAsync(
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

    public async Task<Class> UpdateAsync(UpdateClassDto dto)
    {
        var parameters = new DynamicParameters();
        parameters.Add("ClassId", dto.Id);
        parameters.Add("GroupFk", dto.GroupId);
        parameters.Add("ColorFk", dto.ColorId);
        parameters.Add("SubjectFK", dto.SubjectId);
        parameters.Add("WeekdayFk", dto.WeekdayId);
        parameters.Add("StartsAt", dto.StartsAt);
        parameters.Add("EndsAt", dto.EndsAt);
        parameters.Add("ChangeOn", dto.ChangeOn);

        var classes = await _dbConnection.QueryAsync<
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
