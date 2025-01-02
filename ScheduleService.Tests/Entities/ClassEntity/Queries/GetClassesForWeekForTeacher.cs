using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Common.Specifications.ClassEntity;
using ScheduleService.Application.Contracts.Services;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Teacher;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Teacher.GetClassesForWeekForTeacher;

namespace ScheduleService.Tests.Entities.ClassEntity.Queries;

public class GetClassesForWeekForTeacher
{
    private readonly Mock<IClassService> _mockClassService;
    private readonly Fixture _fixture;
    private readonly GetClassesForWeekForTeacherQuery _query;
    private readonly GetClassesForWeekForTeacherQueryHandler _handler;

    public GetClassesForWeekForTeacher()
    {
        _mockClassService = new Mock<IClassService>();
        _fixture = new Fixture();
        _query = _fixture.Create<GetClassesForWeekForTeacherQuery>();
        _handler = new GetClassesForWeekForTeacherQueryHandler(_mockClassService.Object);
    }

    [Fact]
    public async Task GetClassesForWeekForTeacher_ShouldBe_Success()
    {
        var classes = _fixture
            .CreateMany<
                WeekdayColorClassesDto<
                    ColorClassesDto<TeacherClassDetailDto>,
                    TeacherClassDetailDto
                >
            >(5)
            .ToList();

        _mockClassService
            .Setup(x => x.GetClassesForWeek<TeacherClassDetailDto>(It.IsAny<IClassSpecification>()))
            .ReturnsAsync(classes);

        var result = await _handler.Handle(_query, default);

        _mockClassService.Verify(
            x => x.GetClassesForWeek<TeacherClassDetailDto>(It.IsAny<IClassSpecification>()),
            Times.Once()
        );

        result.Should().NotBeNull();
        result.TeacherId.Should().Be(_query.TeacherId);
        result.Classes.Should().BeEquivalentTo(classes);
    }
}
