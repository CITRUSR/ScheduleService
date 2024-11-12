using MediatR;
using ScheduleService.Application.Common.Models;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.SubjectEntity.Queries.GetSubjects;

public record GetSubjectsQuery(SubjectFilter Filter, PaginationParameters PaginationParameters)
    : IRequest<PagedList<Subject>>;
