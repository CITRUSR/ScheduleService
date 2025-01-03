using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Common.Specifications.ClassEntity;
using ScheduleService.Application.Contracts.Services;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.GetClassesOnCurrentDateForTeacher;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.GetClassOnCurrentDateForStudents;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student.GetClassOnCurrentDateForStudents;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Teacher;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.ClassEntity.Queries;

public class GetClassesOnCurrentDate
{
    private readonly Mock<IClassService> _mockClassService;
    private readonly Fixture _fixture;

    public GetClassesOnCurrentDate()
    {
        _mockClassService = new Mock<IClassService>();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task GetClassesOnCurrentDateForStudent_ShouldBe_Success()
    {
        var (classes, weekday) = SetupClassService<StudentClassDetailDto>();

        var query = _fixture.Create<GetClassesOnCurrentDateForStudentQuery>();
        var handler = new GetClassesOnCurrentDateForStudentQueryHandler(_mockClassService.Object);

        var result = await handler.Handle(query, default);

        Assert<StudentClassDetailDto>();

        result.Classes.Should().BeEquivalentTo(classes);
        result.GroupId.Should().Be(query.GroupId);
        result.Weekday.Should().BeEquivalentTo(weekday);
    }

    [Fact]
    public async Task GetClassesOnCurrentDateForTeacher_ShouldBe_Success()
    {
        var (classes, weekday) = SetupClassService<TeacherClassDetailDto>();

        var query = _fixture.Create<GetClassesOnCurrentDateForTeacherQuery>();
        var handler = new GetClassesOnCurrentDateForTeacherQueryHandler(_mockClassService.Object);

        var result = await handler.Handle(query, default);

        Assert<TeacherClassDetailDto>();

        result.Classes.Should().BeEquivalentTo(classes);
        result.TeacherId.Should().Be(query.TeacherId);
        result.Weekday.Should().BeEquivalentTo(weekday);
    }

    private (List<ColorClassesDto<TClassDetail>>, Weekday) SetupClassService<TClassDetail>()
        where TClassDetail : ClassDetailBase
    {
        var classes = _fixture.CreateMany<ColorClassesDto<TClassDetail>>(5).ToList();

        var weekday = _fixture.Create<Weekday>();

        _mockClassService
            .Setup(x =>
                x.GetClassesForDay<TClassDetail>(It.IsAny<IClassSpecification>(), It.IsAny<int>())
            )
            .ReturnsAsync((classes, weekday));

        return (classes, weekday);
    }

    private void Assert<TClassDetail>()
        where TClassDetail : ClassDetailBase
    {
        _mockClassService.Verify(
            x => x.GetClassesForDay<TClassDetail>(It.IsAny<IClassSpecification>(), It.IsAny<int>()),
            Times.Once()
        );
    }
}
