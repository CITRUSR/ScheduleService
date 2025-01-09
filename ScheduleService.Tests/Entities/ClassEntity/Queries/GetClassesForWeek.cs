using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Common.Specifications.ClassEntity;
using ScheduleService.Application.Contracts.Services;
using ScheduleService.Application.Contracts.UserService.Group;
using ScheduleService.Application.Contracts.UserService.Group.Dto.Responses;
using ScheduleService.Application.Contracts.UserService.Teacher;
using ScheduleService.Application.Contracts.UserService.Teacher.dto.responses;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student.GetClassesForWeekForStudent;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Teacher;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Teacher.GetClassesForWeekForTeacher;

namespace ScheduleService.Tests.Entities.ClassEntity.Queries;

public class GetClassesForWeek
{
    private readonly Mock<IClassService> _mockClassService;
    private readonly Mock<ITeacherService> _mockTeacherService;
    private readonly Mock<IGroupService> _mockGroupService;
    private readonly Fixture _fixture;

    public GetClassesForWeek()
    {
        _mockTeacherService = new Mock<ITeacherService>();
        _mockGroupService = new Mock<IGroupService>();
        _mockClassService = new Mock<IClassService>();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task GetClassesForWeekForStudent_ShouldBe_Success()
    {
        var classes = SetupClassService<StudentClassDetailDto>();

        var query = _fixture.Create<GetClassesForWeekForStudentQuery>();

        var group = _fixture.Build<GroupDto>().With(x => x.Id, query.GroupId).Create();

        _mockGroupService.Setup(x => x.GetGroupById(query.GroupId)).ReturnsAsync(group);

        var handler = new GetClassesForWeekForStudentQueryHandler(
            _mockClassService.Object,
            _mockTeacherService.Object,
            _mockGroupService.Object
        );

        var result = await handler.Handle(query, CancellationToken.None);

        _mockGroupService.Verify(x => x.GetGroupById(query.GroupId), Times.Once());

        Assert<StudentClassDetailDto>();

        result.Classes.Should().BeEquivalentTo(classes);
        result.Group.Id.Should().Be(query.GroupId);
    }

    [Fact]
    public async Task GetClassesForWeekForTeacher_ShouldBe_Success()
    {
        var classes = SetupClassService<TeacherClassDetailDto>();

        var query = _fixture.Create<GetClassesForWeekForTeacherQuery>();

        var teacher = _fixture.Build<TeacherDto>().With(x => x.Id, query.TeacherId).Create();

        _mockTeacherService.Setup(x => x.GetTeacherById(query.TeacherId)).ReturnsAsync(teacher);

        var handler = new GetClassesForWeekForTeacherQueryHandler(
            _mockClassService.Object,
            _mockGroupService.Object,
            _mockTeacherService.Object
        );

        var result = await handler.Handle(query, CancellationToken.None);

        _mockTeacherService.Verify(x => x.GetTeacherById(query.TeacherId), Times.Once());

        Assert<TeacherClassDetailDto>();

        result.Classes.Should().BeEquivalentTo(classes);
        result.Teacher.Id.Should().Be(query.TeacherId);
    }

    private List<
        WeekdayColorClassesDto<ColorClassesDto<TClassDetail>, TClassDetail>
    > SetupClassService<TClassDetail>()
        where TClassDetail : ClassDetailBase
    {
        var classes = _fixture
            .CreateMany<WeekdayColorClassesDto<ColorClassesDto<TClassDetail>, TClassDetail>>(5)
            .ToList();

        _mockClassService
            .Setup(x => x.GetClassesForWeek<TClassDetail>(It.IsAny<IClassSpecification>()))
            .ReturnsAsync(classes);

        return classes;
    }

    private void Assert<TClassDetail>()
        where TClassDetail : ClassDetailBase
    {
        _mockClassService.Verify(
            x => x.GetClassesForWeek<TClassDetail>(It.IsAny<IClassSpecification>()),
            Times.Once()
        );
    }
}
