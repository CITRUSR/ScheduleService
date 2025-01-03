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
        var subject = _fixture.Create<Subject>();

        _mockUnitOfWork
            .Setup(x => x.SubjectRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(subject);

        var result = await _handler.Handle(_query, default);

        _mockUnitOfWork.Verify(
            x => x.SubjectRepository.GetByIdAsync(It.IsAny<int>()),
            Times.Once()
        );

        result.Id.Should().Be(subject.Id);
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
