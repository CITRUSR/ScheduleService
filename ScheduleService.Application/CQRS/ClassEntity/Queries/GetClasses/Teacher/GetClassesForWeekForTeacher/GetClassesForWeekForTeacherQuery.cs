using MediatR;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Teacher.GetClassesForWeekForTeacher;

public record GetClassesForWeekForTeacherQuery(Guid TeacherId)
    : IRequest<GetClassesForWeekForTeacherResponse>;
