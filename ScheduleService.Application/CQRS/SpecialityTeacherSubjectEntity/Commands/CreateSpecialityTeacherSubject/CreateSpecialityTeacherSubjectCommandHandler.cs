using MediatR;
using Npgsql;
using ScheduleService.Application.Common.Exceptions.SpecialityTeacherSubjectEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.Contracts.UserService.Speciality;
using ScheduleService.Application.Contracts.UserService.Teacher;
using ScheduleService.Application.CQRS.SubjectEntity.Queries.GetSubjectById;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.SpecialityTeacherSubjectEntity.Commands.CreateSpecialityTeacherSubject;

public class CreateSpecialityTeacherSubjectCommandHandler(
    IUnitOfWork unitOfWork,
    IMediator mediator,
    ITeacherService teacherService,
    ISpecialityService specialityService
) : IRequestHandler<CreateSpecialityTeacherSubjectCommand, SpecialityTeacherSubject>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMediator _mediator = mediator;
    private readonly ITeacherService _teacherService = teacherService;
    private readonly ISpecialityService _specialityService = specialityService;

    public async Task<SpecialityTeacherSubject> Handle(
        CreateSpecialityTeacherSubjectCommand request,
        CancellationToken cancellationToken
    )
    {
        await _mediator.Send(new GetSubjectByIdQuery(request.SubjectId), cancellationToken);

        List<Task> checkTasks =
        [
            _teacherService.GetTeacherById(request.TeacherId),
            _specialityService.GetSpecialityById(request.SpecialityId),
        ];

        await Task.WhenAll(checkTasks);

        var entity = new SpecialityTeacherSubject()
        {
            SpecialityId = request.SpecialityId,
            Course = request.Course,
            TeacherId = request.TeacherId,
            SubjectId = request.SubjectId,
            SubGroup = request.SubGroup,
        };

        try
        {
            await _unitOfWork.SpecialityTeacherSubjectRepository.InsertAsync(entity);

            _unitOfWork.CommitTransaction();
        }
        catch (Exception ex)
        {
            if (ex is PostgresException postgresException)
            {
                if (postgresException.SqlState == "23505")
                {
                    if (postgresException.Detail.Contains("speciality_fk"))
                    {
                        throw new PrimarySpecialityTeacherSubjectAlreadyExistsException(
                            request.SpecialityId,
                            request.Course,
                            request.SubGroup
                        );
                    }
                    else if (postgresException.Detail.Contains("speciality_fk"))
                    {
                        if (postgresException.Detail.Contains("teacher_fk"))
                        {
                            throw new TeacherSubjectCombinationAlreadyExistsException(
                                request.TeacherId,
                                request.SubjectId
                            );
                        }
                    }
                }
            }

            throw;
        }

        return entity;
    }
}
