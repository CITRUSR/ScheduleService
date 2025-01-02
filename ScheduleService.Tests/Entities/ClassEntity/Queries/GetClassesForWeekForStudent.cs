using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Common.Specifications.ClassEntity;
using ScheduleService.Application.Contracts.Services;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student.GetClassesForWeekForStudent;

namespace ScheduleService.Tests.Entities.ClassEntity.Queries;

public class GetClassesForWeekForStudent
{
    private readonly Mock<IClassService> _mockClassService;
    private readonly Fixture _fixture;
    private readonly GetClassesForWeekForStudentQuery _query;
    private readonly GetClassesForWeekForStudentQueryHandler _handler;

    public GetClassesForWeekForStudent()
    {
        _mockClassService = new Mock<IClassService>();
        _fixture = new Fixture();
        _query = _fixture.Create<GetClassesForWeekForStudentQuery>();
        _handler = new GetClassesForWeekForStudentQueryHandler(_mockClassService.Object);
    }

    [Fact]
    public async Task GetClassesForWeekForStudent_ShouldBe_Success()
    {
        var classes = _fixture
            .CreateMany<
                WeekdayColorClassesDto<
                    ColorClassesDto<StudentClassDetailDto>,
                    StudentClassDetailDto
                >
            >(5)
            .ToList();

        _mockClassService
            .Setup(x => x.GetClassesForWeek<StudentClassDetailDto>(It.IsAny<IClassSpecification>()))
            .ReturnsAsync(classes);

        var result = await _handler.Handle(_query, default);

        _mockClassService.Verify(
            x => x.GetClassesForWeek<StudentClassDetailDto>(It.IsAny<IClassSpecification>()),
            Times.Once()
        );

        result.Should().NotBeNull();
        result.GroupId.Should().Be(_query.GroupId);
        result.Classes.Should().BeEquivalentTo(classes);
    }
}
