using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.SubjectEntity.Queries.GetSubjectById;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.SubjectEntity.Queries;

public class GetSubjectById
{
    private readonly Fixture _fixture;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly GetSubjectByIdQueryHandler _handler;
    private readonly GetSubjectByIdQuery _query;

    public GetSubjectById()
    {
        _fixture = new Fixture();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new GetSubjectByIdQueryHandler(_mockUnitOfWork.Object);
        _query = _fixture.Create<GetSubjectByIdQuery>();
    }

    [Fact]
    public async Task GetSubjectById_ShouldBe_Success()
    {
        _mockUnitOfWork
            .Setup(x => x.SubjectRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Subject());

        var result = await _handler.Handle(_query, default);

        _mockUnitOfWork.Verify(
            x => x.SubjectRepository.GetByIdAsync(It.IsAny<int>()),
            Times.Once()
        );

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetSubjectById_ShouldBe_SubjectNotFoundException()
    {
        _mockUnitOfWork
            .Setup(x => x.SubjectRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Subject?)null);

        Func<Task> act = async () => await _handler.Handle(_query, default);

        await act.Should().ThrowAsync<SubjectNotFoundException>();
    }
}
