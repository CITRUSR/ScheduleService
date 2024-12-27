using MediatR;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.GetClassOnCurrentDateForStudents;

public record GetClassesOnCurrentDateForStudentQuery(int GroupId)
    : IRequest<GetClassesOnCurrentDateForStudentResponse>;
