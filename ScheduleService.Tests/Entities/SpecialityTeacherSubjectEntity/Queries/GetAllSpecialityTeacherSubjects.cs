using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.SpecialityTeacherSubjectEntity.Queries.GetAllSpecialityTeacherSubjects;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.SpecialityTeacherSubjectEntity.Queries;

public class GetAllSpecialityTeacherSubjects
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly IFixture _fixture;

    public GetAllSpecialityTeacherSubjects()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task GetAllSpecialityTeacherSubjects_ShouldBe_Success()
    {
        var entities = _fixture.CreateMany<SpecialityTeacherSubject>(15).ToList();

        _mockUnitOfWork
            .Setup(x => x.SpecialityTeacherSubjectRepository.GetAllAsync())
            .ReturnsAsync(entities);

        var handler = new GetAllSpecialityTeacherSubjectsQueryHandler(_mockUnitOfWork.Object);

        var result = await handler.Handle(new GetAllSpecialityTeacherSubjectsQuery(), default);

        _mockUnitOfWork.Verify(
            x => x.SpecialityTeacherSubjectRepository.GetAllAsync(),
            Times.Once()
        );

        result.Count.Should().Be(entities.Count);
    }
}
