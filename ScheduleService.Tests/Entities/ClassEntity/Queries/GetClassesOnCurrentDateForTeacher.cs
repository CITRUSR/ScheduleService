using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Common.Specifications.ClassEntity;
using ScheduleService.Application.Contracts.Services;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.GetClassOnCurrentDateForStudents;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student.GetClassOnCurrentDateForStudents;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Teacher;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.ClassEntity.Queries;

public class GetClassesOnCurrentDateForTeacher
{
    private readonly Mock<IClassService> _mockClassService;
    private readonly Fixture _fixture;
    private readonly GetClassesOnCurrentDateForStudentQuery _query;
    private readonly GetClassesOnCurrentDateForStudentQueryHandler _handler;

    public GetClassesOnCurrentDateForTeacher()
    {
        _mockClassService = new Mock<IClassService>();
        _fixture = new Fixture();
        _query = _fixture.Create<GetClassesOnCurrentDateForStudentQuery>();
        _handler = new GetClassesOnCurrentDateForStudentQueryHandler(_mockClassService.Object);
    }

    [Fact]
    public async Task GetClassesOnCurrentDateForTeacher_ShouldBe_Success()
    {
        var classes = _fixture.CreateMany<ColorClassesDto<TeacherClassDetailDto>>(5).ToList();

        var currentWeekdayOrder = 2;

        var weekday = _fixture.Build<Weekday>().With(x => x.Id, currentWeekdayOrder).Create();

        _mockClassService
            .Setup(x =>
                x.GetClassesForDay<TeacherClassDetailDto>(
                    It.IsAny<IClassSpecification>(),
                    currentWeekdayOrder
                )
            )
            .ReturnsAsync((classes, weekday));

        var result = await _handler.Handle(_query, default);

        _mockClassService.Verify(
            x =>
                x.GetClassesForDay<TeacherClassDetailDto>(
                    It.IsAny<IClassSpecification>(),
                    currentWeekdayOrder
                ),
            Times.Once()
        );

        result.Should().NotBeNull();
        result.Classes.Should().BeEquivalentTo(classes);
        result.GroupId.Should().Be(_query.GroupId);
        result.Weekday.Should().BeEquivalentTo(weekday);
    }
}
