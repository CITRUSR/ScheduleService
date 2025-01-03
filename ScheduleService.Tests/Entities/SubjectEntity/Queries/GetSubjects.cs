using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Common.Models;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.SubjectEntity.Queries.GetSubjects;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.SubjectEntity.Queries;

public class GetSubjects
{
    private readonly Fixture _fixture;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    public GetSubjects()
    {
        _fixture = new Fixture();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
    }

    [Fact]
    public async Task GetSubjects_ShouldBe_Success()
    {
        var query = _fixture.Create<GetSubjectsQuery>();
        var handler = new GetSubjectsQueryHandler(_mockUnitOfWork.Object);

        var subjects = _fixture.CreateMany<Subject>(10);

        _mockUnitOfWork
            .Setup(u => u.SubjectRepository.GetAsync(query.Filter, query.PaginationParameters))
            .ReturnsAsync(new PagedList<Subject>([.. subjects], 1, 1, 1));

        var result = await handler.Handle(query, CancellationToken.None);

        _mockUnitOfWork.Verify(
            x =>
                x.SubjectRepository.GetAsync(
                    It.IsAny<SubjectFilter>(),
                    It.IsAny<PaginationParameters>()
                ),
            Times.Once()
        );

        result.Items.Should().HaveCount(10);
    }
}
