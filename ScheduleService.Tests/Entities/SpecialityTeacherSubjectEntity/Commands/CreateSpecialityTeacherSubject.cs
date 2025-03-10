using AutoFixture;
using FluentAssertions;
using Grpc.Core;
using MediatR;
using Moq;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.Contracts.UserService.Speciality;
using ScheduleService.Application.Contracts.UserService.Speciality.dto.Responses;
using ScheduleService.Application.Contracts.UserService.Teacher;
using ScheduleService.Application.Contracts.UserService.Teacher.dto.responses;
using ScheduleService.Application.CQRS.SpecialityTeacherSubjectEntity.Commands.CreateSpecialityTeacherSubject;
using ScheduleService.Application.CQRS.SubjectEntity.Queries.GetSubjectById;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.SpecialityTeacherSubjectEntity.Commands;

public class CreateSpecialityTeacherSubject
{
    private readonly IFixture _fixture;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ISpecialityService> _mockSpecialityService;
    private readonly Mock<ITeacherService> _mockTeacherService;
    private readonly Mock<IMediator> _mockMediator;
    private readonly CreateSpecialityTeacherSubjectCommandHandler _handler;

    public CreateSpecialityTeacherSubject()
    {
        _fixture = new Fixture();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockSpecialityService = new Mock<ISpecialityService>();
        _mockTeacherService = new Mock<ITeacherService>();
        _mockMediator = new Mock<IMediator>();
        _handler = new CreateSpecialityTeacherSubjectCommandHandler(
            _mockUnitOfWork.Object,
            _mockMediator.Object,
            _mockTeacherService.Object,
            _mockSpecialityService.Object
        );
    }

    [Fact]
    public async Task CreateSpecialityTeacherSubject_ShouldBe_Success()
    {
        var (entity, subject, speciality, teacher, subjectQuery) = GetEntities();

        _mockUnitOfWork
            .Setup(x => x.SpecialityTeacherSubjectRepository.InsertAsync(entity))
            .ReturnsAsync(entity);

        _mockMediator
            .Setup(x => x.Send(subjectQuery, It.IsAny<CancellationToken>()))
            .ReturnsAsync(subject);

        _mockTeacherService.Setup(x => x.GetTeacherById(teacher.Id)).ReturnsAsync(teacher);

        _mockSpecialityService
            .Setup(x => x.GetSpecialityById(speciality.Id))
            .ReturnsAsync(speciality);

        var command = new CreateSpecialityTeacherSubjectCommand(
            entity.SpecialityId,
            entity.Course,
            entity.SubGroup,
            entity.TeacherId,
            entity.SubjectId
        );

        var res = await _handler.Handle(command, default);

        _mockMediator.Verify(
            x => x.Send(subjectQuery, It.IsAny<CancellationToken>()),
            Times.Once()
        );

        _mockSpecialityService.Verify(x => x.GetSpecialityById(entity.SpecialityId), Times.Once());

        _mockTeacherService.Verify(x => x.GetTeacherById(teacher.Id), Times.Once());

        _mockUnitOfWork.Verify(
            x =>
                x.SpecialityTeacherSubjectRepository.InsertAsync(
                    It.Is<SpecialityTeacherSubject>(x =>
                        x.Course == entity.Course
                        && x.SubGroup == entity.SubGroup
                        && x.SpecialityId == entity.SpecialityId
                        && x.SubjectId == entity.SubjectId
                        && x.TeacherId == entity.TeacherId
                    )
                ),
            Times.Once()
        );

        _mockUnitOfWork.Verify(x => x.CommitTransaction(), Times.Once());

        res.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateSpecialityTeacherSubject_ShouldBe_SpecialityNotFoundException()
    {
        var (entity, subject, speciality, teacher, subjectQuery) = GetEntities();

        _mockUnitOfWork.Setup(x => x.SpecialityTeacherSubjectRepository.InsertAsync(entity));

        _mockMediator
            .Setup(x => x.Send(subjectQuery, It.IsAny<CancellationToken>()))
            .ReturnsAsync(subject);

        _mockTeacherService.Setup(x => x.GetTeacherById(teacher.Id)).ReturnsAsync(teacher);

        _mockSpecialityService
            .Setup(x => x.GetSpecialityById(speciality.Id))
            .Throws(() => new RpcException(new Status(StatusCode.NotFound, "")));

        var command = new CreateSpecialityTeacherSubjectCommand(
            entity.SpecialityId,
            entity.Course,
            entity.SubGroup,
            entity.TeacherId,
            entity.SubjectId
        );

        Func<Task> act = async () => await _handler.Handle(command, default);

        await act.Should().ThrowAsync<RpcException>();

        _mockMediator.Verify(
            x => x.Send(subjectQuery, It.IsAny<CancellationToken>()),
            Times.Once()
        );

        _mockSpecialityService.Verify(x => x.GetSpecialityById(entity.SpecialityId), Times.Once());

        _mockTeacherService.Verify(x => x.GetTeacherById(teacher.Id), Times.Once());

        _mockUnitOfWork.Verify(
            x => x.SpecialityTeacherSubjectRepository.InsertAsync(entity),
            Times.Never()
        );

        _mockUnitOfWork.Verify(x => x.CommitTransaction(), Times.Never());
    }

    [Fact]
    public async Task CreateSpecialityTeacherSubject_ShouldBe_TeacherNotFoundException()
    {
        var (entity, subject, speciality, teacher, subjectQuery) = GetEntities();

        _mockUnitOfWork.Setup(x => x.SpecialityTeacherSubjectRepository.InsertAsync(entity));

        _mockMediator
            .Setup(x => x.Send(subjectQuery, It.IsAny<CancellationToken>()))
            .ReturnsAsync(subject);

        _mockTeacherService
            .Setup(x => x.GetTeacherById(teacher.Id))
            .Throws(new RpcException(new Status(StatusCode.NotFound, "")));

        _mockSpecialityService
            .Setup(x => x.GetSpecialityById(speciality.Id))
            .ReturnsAsync(speciality);

        var command = new CreateSpecialityTeacherSubjectCommand(
            entity.SpecialityId,
            entity.Course,
            entity.SubGroup,
            entity.TeacherId,
            entity.SubjectId
        );

        Func<Task> act = async () => await _handler.Handle(command, default);

        await act.Should().ThrowAsync<RpcException>();

        _mockMediator.Verify(
            x => x.Send(subjectQuery, It.IsAny<CancellationToken>()),
            Times.Once()
        );

        _mockSpecialityService.Verify(x => x.GetSpecialityById(entity.SpecialityId), Times.Never());

        _mockTeacherService.Verify(x => x.GetTeacherById(teacher.Id), Times.Once());

        _mockUnitOfWork.Verify(
            x => x.SpecialityTeacherSubjectRepository.InsertAsync(entity),
            Times.Never()
        );

        _mockUnitOfWork.Verify(x => x.CommitTransaction(), Times.Never());
    }

    [Fact]
    public async Task CreateSpecialityTeacherSubject_ShouldBe_SubjectNotFoundException()
    {
        var (entity, subject, speciality, teacher, subjectQuery) = GetEntities();

        _mockUnitOfWork.Setup(x => x.SpecialityTeacherSubjectRepository.InsertAsync(entity));

        _mockMediator
            .Setup(x => x.Send(subjectQuery, It.IsAny<CancellationToken>()))
            .Throws(new SubjectNotFoundException(subject.Id));

        _mockTeacherService.Setup(x => x.GetTeacherById(teacher.Id)).ReturnsAsync(teacher);

        _mockSpecialityService
            .Setup(x => x.GetSpecialityById(speciality.Id))
            .ReturnsAsync(speciality);

        var command = new CreateSpecialityTeacherSubjectCommand(
            entity.SpecialityId,
            entity.Course,
            entity.SubGroup,
            entity.TeacherId,
            entity.SubjectId
        );

        Func<Task> act = async () => await _handler.Handle(command, default);

        await act.Should().ThrowAsync<SubjectNotFoundException>();

        _mockMediator.Verify(
            x => x.Send(subjectQuery, It.IsAny<CancellationToken>()),
            Times.Once()
        );

        _mockSpecialityService.Verify(x => x.GetSpecialityById(entity.SpecialityId), Times.Never());

        _mockTeacherService.Verify(x => x.GetTeacherById(teacher.Id), Times.Never());

        _mockUnitOfWork.Verify(
            x => x.SpecialityTeacherSubjectRepository.InsertAsync(entity),
            Times.Never()
        );

        _mockUnitOfWork.Verify(x => x.CommitTransaction(), Times.Never());
    }

    private (
        SpecialityTeacherSubject entity,
        Subject subject,
        SpecialityDto speciality,
        TeacherDto teacher,
        GetSubjectByIdQuery subjectQuery
    ) GetEntities()
    {
        var entity = _fixture.Create<SpecialityTeacherSubject>();

        var subject = _fixture.Build<Subject>().With(x => x.Id, entity.SubjectId).Create();
        var speciality = _fixture
            .Build<SpecialityDto>()
            .With(x => x.Id, entity.SpecialityId)
            .Create();
        var teacher = _fixture.Build<TeacherDto>().With(x => x.Id, entity.TeacherId).Create();

        var subjectQuery = new GetSubjectByIdQuery(entity.SubjectId);

        return (entity, subject, speciality, teacher, subjectQuery);
    }
}
