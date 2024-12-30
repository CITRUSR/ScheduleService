using MediatR;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.GetClassOnCurrentDateForStudents;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student.GetClassOnCurrentDateForStudents;

public record GetClassesOnCurrentDateForStudentQuery(int GroupId)
    : IRequest<GetClassesOnCurrentDateForStudentResponse>;
