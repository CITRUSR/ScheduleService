using AutoFixture;
using FluentAssertions;
using Grpc.Core;
using Moq;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.Contracts.UserService.Group;
using ScheduleService.Application.Contracts.UserService.Group.Dto.Responses;
using ScheduleService.Application.Contracts.UserService.Teacher;
using ScheduleService.Application.Contracts.UserService.Teacher.dto.responses;
using ScheduleService.Application.CQRS.ClassEntity.Commands.CreateClass;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.ClassEntity.Commands;

public class CreateClass
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IGroupService> _mockGroupService;
    private readonly Mock<ITeacherService> _mockTeacherService;
    private readonly CreateClassCommandHandler _handler;
    private readonly CreateClassCommand _command;
    private readonly Fixture _fixture;

    public CreateClass()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockGroupService = new Mock<IGroupService>();
        _mockTeacherService = new Mock<ITeacherService>();
        _fixture = new Fixture();
        _command = _fixture.Create<CreateClassCommand>();
        _handler = new CreateClassCommandHandler(
            _mockUnitOfWork.Object,
            _mockGroupService.Object,
            _mockTeacherService.Object
        );
    }

    [Fact]
    public async Task CreateClass_ShouldBe_Success()
    {
        var classCreationDto = _fixture.Create<ClassCreationDto>();

        SetupGetEntitiesForInsertClassAsync(classCreationDto);

        _mockTeacherService
            .Setup(x => x.GetTeacherById(It.IsAny<Guid>()))
            .ReturnsAsync(new TeacherDto());

        _mockGroupService.Setup(x => x.GetGroupById(It.IsAny<int>())).ReturnsAsync(new GroupDto());

        _mockUnitOfWork
            .Setup(x => x.ClassRepository.InsertAsync(It.IsAny<CreateClassDto>()))
            .ReturnsAsync(new Class());

        var result = await _handler.Handle(_command, default);

        _mockUnitOfWork.Verify(
            x => x.ClassRepository.GetEntitiesForInsertClassAsync(It.IsAny<CreateClassDto>()),
            Times.Once()
        );

        _mockTeacherService.Verify(
            x => x.GetTeacherById(It.IsAny<Guid>()),
            Times.Exactly(_command.TeachersIds.Count)
        );

        _mockGroupService.Verify(x => x.GetGroupById(It.IsAny<int>()), Times.Once());

        _mockUnitOfWork.Verify(
            x => x.ClassRepository.InsertAsync(It.IsAny<CreateClassDto>()),
            Times.Once()
        );

        _mockUnitOfWork.Verify(x => x.CommitTransaction(), Times.Once());

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateClass_ShouldBe_ColorNotFoundException()
    {
        var classCreationDto = _fixture
            .Build<ClassCreationDto>()
            .With(x => x.Color, (Color?)null)
            .Create();

        SetupGetEntitiesForInsertClassAsync(classCreationDto);

        Func<Task> act = async () => await _handler.Handle(_command, default);

        await act.Should().ThrowAsync<ColorNotFoundException>();
    }

    [Fact]
    public async Task CreateClass_ShouldBe_SubjectNotFoundException()
    {
        var classCreationDto = _fixture
            .Build<ClassCreationDto>()
            .With(x => x.Subject, (Subject?)null)
            .Create();

        SetupGetEntitiesForInsertClassAsync(classCreationDto);

        Func<Task> act = async () => await _handler.Handle(_command, default);

        await act.Should().ThrowAsync<SubjectNotFoundException>();
    }

    [Fact]
    public async Task CreateClass_ShouldBe_WeekdayNotFoundException()
    {
        var classCreationDto = _fixture
            .Build<ClassCreationDto>()
            .With(x => x.Weekday, (Weekday?)null)
            .Create();

        SetupGetEntitiesForInsertClassAsync(classCreationDto);

        Func<Task> act = async () => await _handler.Handle(_command, default);

        await act.Should().ThrowAsync<WeekdayNotFoundException>();
    }

    [Fact]
    public async Task CreateClass_ShouldBe_RoomsNotFoundException()
    {
        var rooms = _fixture.CreateMany<Room>(5);

        var classCreationDto = _fixture
            .Build<ClassCreationDto>()
            .With(x => x.Rooms, [.. rooms])
            .Create();

        SetupGetEntitiesForInsertClassAsync(classCreationDto);

        Func<Task> act = async () =>
            await _handler.Handle(
                _command with
                {
                    RoomIds = [.. rooms.Select(x => x.Id).Take(2)]
                },
                default
            );

        await act.Should().ThrowAsync<RoomNotFoundException>();
    }

    [Fact]
    public async Task CreateClass_ShouldBe_TeachersNotFoundException()
    {
        var classCreationDto = _fixture.Create<ClassCreationDto>();

        SetupGetEntitiesForInsertClassAsync(classCreationDto);

        _mockTeacherService
            .Setup(x => x.GetTeacherById(It.IsAny<Guid>()))
            .ThrowsAsync(new RpcException(new Status(StatusCode.NotFound, "")));

        Func<Task> act = async () => await _handler.Handle(_command, default);

        await act.Should().ThrowAsync<RpcException>();
    }

    [Fact]
    public async Task CreateClass_ShouldBe_GroupNotFoundException()
    {
        var classCreationDto = _fixture.Create<ClassCreationDto>();

        SetupGetEntitiesForInsertClassAsync(classCreationDto);

        _mockTeacherService
            .Setup(x => x.GetTeacherById(It.IsAny<Guid>()))
            .ReturnsAsync(new TeacherDto());

        _mockGroupService
            .Setup(x => x.GetGroupById(It.IsAny<int>()))
            .ThrowsAsync(new RpcException(new Status(StatusCode.NotFound, "")));

        Func<Task> act = async () => await _handler.Handle(_command, default);

        await act.Should().ThrowAsync<RpcException>();
    }

    private void SetupGetEntitiesForInsertClassAsync(ClassCreationDto dto)
    {
        _mockUnitOfWork
            .Setup(x =>
                x.ClassRepository.GetEntitiesForInsertClassAsync(It.IsAny<CreateClassDto>())
            )
            .ReturnsAsync(dto);
    }
}
