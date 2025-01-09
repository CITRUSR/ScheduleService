using AutoFixture;
using FluentAssertions;
using Grpc.Core;
using MediatR;
using Moq;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Common.Exceptions.ClassEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.Contracts.UserService.Group;
using ScheduleService.Application.Contracts.UserService.Group.Dto.Responses;
using ScheduleService.Application.Contracts.UserService.Teacher;
using ScheduleService.Application.Contracts.UserService.Teacher.dto.responses;
using ScheduleService.Application.CQRS.ClassEntity;
using ScheduleService.Application.CQRS.ClassEntity.Commands.UpdateClass;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClassById;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.ClassEntity.Commands;

public class UpdateClass
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IGroupService> _mockGroupService;
    private readonly Mock<ITeacherService> _mockTeacherService;
    private readonly Mock<IMediator> _mockMediator;
    private readonly UpdateClassCommandHandler _handler;
    private readonly UpdateClassCommand _command;
    private readonly Fixture _fixture;

    public UpdateClass()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockGroupService = new Mock<IGroupService>();
        _mockTeacherService = new Mock<ITeacherService>();
        _mockMediator = new Mock<IMediator>();
        _fixture = new Fixture();
        _handler = new UpdateClassCommandHandler(
            _mockUnitOfWork.Object,
            _mockGroupService.Object,
            _mockTeacherService.Object,
            _mockMediator.Object
        );
        _command = _fixture.Create<UpdateClassCommand>();
    }

    [Fact]
    public async Task UpdateClass_ShouldBe_Success()
    {
        var dependencies = _fixture.Create<ClassDependenciesDto>();

        var @class = _fixture.Create<Class>();

        SetupGetClassDependencies(dependencies);
        SetupMediator();

        _mockTeacherService
            .Setup(x => x.GetTeacherById(It.IsAny<Guid>()))
            .ReturnsAsync(new TeacherDto());

        _mockGroupService.Setup(x => x.GetGroupById(It.IsAny<int>())).ReturnsAsync(new GroupDto());

        _mockUnitOfWork
            .Setup(x => x.ClassRepository.UpdateAsync(It.IsAny<UpdateClassDto>()))
            .ReturnsAsync(@class);

        var result = await _handler.Handle(_command, default);

        _mockUnitOfWork.Verify(
            x => x.ClassRepository.GetClassDependencies(It.IsAny<GetClassDependenciesDto>()),
            Times.Once()
        );

        _mockTeacherService.Verify(
            x => x.GetTeacherById(It.IsAny<Guid>()),
            Times.Exactly(_command.TeacherIds.Count)
        );

        _mockGroupService.Verify(x => x.GetGroupById(It.IsAny<int>()), Times.Once());

        _mockUnitOfWork.Verify(
            x => x.ClassRepository.UpdateAsync(It.IsAny<UpdateClassDto>()),
            Times.Once()
        );

        _mockMediator.Verify(
            x => x.Send(It.IsAny<GetClassByIdQuery>(), It.IsAny<CancellationToken>()),
            Times.Once()
        );

        _mockUnitOfWork.Verify(x => x.CommitTransaction(), Times.Once());

        result.Id.Should().Be(@class.Id);
    }

    [Fact]
    public async Task UpdateClass_ShouldBe_SubjectNotFoundException()
    {
        var dependencies = _fixture
            .Build<ClassDependenciesDto>()
            .With(x => x.Subject, (Subject?)null)
            .Create();

        SetupGetClassDependencies(dependencies);
        SetupMediator();

        Func<Task> act = async () => await _handler.Handle(_command, default);

        await act.Should().ThrowAsync<SubjectNotFoundException>();
    }

    [Fact]
    public async Task UpdateClass_ShouldBe_ColorNotFoundException()
    {
        var dependencies = _fixture
            .Build<ClassDependenciesDto>()
            .With(x => x.Color, (Color?)null)
            .Create();

        SetupGetClassDependencies(dependencies);
        SetupMediator();

        Func<Task> act = async () => await _handler.Handle(_command, default);

        await act.Should().ThrowAsync<ColorNotFoundException>();
    }

    [Fact]
    public async Task UpdateClass_ShouldBe_WeekdayNotFoundException()
    {
        var dependencies = _fixture
            .Build<ClassDependenciesDto>()
            .With(x => x.Weekday, (Weekday?)null)
            .Create();

        SetupGetClassDependencies(dependencies);
        SetupMediator();

        Func<Task> act = async () => await _handler.Handle(_command, default);

        await act.Should().ThrowAsync<WeekdayNotFoundException>();
    }

    [Fact]
    public async Task UpdateClass_ShouldBe_RoomNotFoundException()
    {
        var roomsIds = _fixture.CreateMany<int>(5);

        var dependencies = _fixture.Build<ClassDependenciesDto>().With(x => x.Rooms, []).Create();

        SetupGetClassDependencies(dependencies);
        SetupMediator();

        Func<Task> act = async () =>
            await _handler.Handle(_command with { RoomIds = [.. roomsIds] }, default);

        await act.Should().ThrowAsync<RoomNotFoundException>();
    }

    [Fact]
    public async Task UpdateClass_ShouldBe_TeacherNotFoundException()
    {
        var teachersIds = _fixture.CreateMany<Guid>(5);

        var dependencies = _fixture.Create<ClassDependenciesDto>();

        SetupGetClassDependencies(dependencies);
        SetupMediator();

        _mockTeacherService
            .Setup(x => x.GetTeacherById(It.IsAny<Guid>()))
            .ThrowsAsync(new RpcException(new Status(StatusCode.NotFound, "")));

        Func<Task> act = async () =>
            await _handler.Handle(_command with { TeacherIds = [.. teachersIds] }, default);

        await act.Should().ThrowAsync<RpcException>();
    }

    [Fact]
    public async Task UpdateClass_ShouldBe_GroupNotFoundException()
    {
        var dependencies = _fixture.Create<ClassDependenciesDto>();

        SetupGetClassDependencies(dependencies);
        SetupMediator();

        _mockTeacherService
            .Setup(x => x.GetTeacherById(It.IsAny<Guid>()))
            .ReturnsAsync(new TeacherDto());

        _mockGroupService
            .Setup(x => x.GetGroupById(It.IsAny<int>()))
            .ThrowsAsync(new RpcException(new Status(StatusCode.NotFound, "")));

        Func<Task> act = async () => await _handler.Handle(_command, default);

        await act.Should().ThrowAsync<RpcException>();
    }

    [Fact]
    public async Task UpdateClass_ShouldBe_ClassNotFoundException()
    {
        Func<Task> act = async () => await _handler.Handle(_command, default);

        await act.Should().ThrowAsync<ClassNotFoundException>();
    }

    private void SetupGetClassDependencies(ClassDependenciesDto dto)
    {
        _mockUnitOfWork
            .Setup(x => x.ClassRepository.GetClassDependencies(It.IsAny<GetClassDependenciesDto>()))
            .ReturnsAsync(dto);
    }

    private void SetupMediator()
    {
        var @class = _fixture.Build<Class>().With(x => x.Id, _command.ClassId).Create();

        _mockMediator
            .Setup(x => x.Send(It.IsAny<GetClassByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(@class);
    }
}
