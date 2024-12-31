using MediatR;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student.GetClassesForWeekForStudent;

public record GetClassesForWeekForStudentQuery(int GroupId)
    : IRequest<GetClassesForWeekForStudentResponse>;
