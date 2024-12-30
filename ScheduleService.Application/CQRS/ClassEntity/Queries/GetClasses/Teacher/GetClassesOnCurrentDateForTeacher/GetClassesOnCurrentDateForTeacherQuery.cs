using MediatR;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.GetClassesOnCurrentDateForTeacher;

public record GetClassesOnCurrentDateForTeacherQuery(Guid TeacherId)
    : IRequest<GetClassesOnCurrentDateForTeacherResponse>;
