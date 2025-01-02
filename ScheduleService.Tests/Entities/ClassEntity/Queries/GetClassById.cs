using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Common.Exceptions.ClassEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClassById;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.ClassEntity.Queries;

public class GetClassById
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Fixture _fixture;
    private readonly GetClassByIdQuery _query;
    private readonly GetClassByIdQueryHandler _handler;

    public GetClassById()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _fixture = new Fixture();
        _query = _fixture.Create<GetClassByIdQuery>();
        _handler = new GetClassByIdQueryHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task GetClassById_ShouldBe_Success()
    {
        _mockUnitOfWork
            .Setup(x => x.ClassRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Class());

        var res = await _handler.Handle(_query, default);

        _mockUnitOfWork.Verify(x => x.ClassRepository.GetByIdAsync(It.IsAny<int>()), Times.Once);

        res.Should().NotBeNull();
    }

    [Fact]
    public async Task GetClassById_ShouldBe_ClassNotFoundException()
    {
        _mockUnitOfWork
            .Setup(x => x.ClassRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Class?)null);

        Func<Task> act = async () => await _handler.Handle(_query, default);

        await act.Should().ThrowAsync<ClassNotFoundException>();
    }
}
