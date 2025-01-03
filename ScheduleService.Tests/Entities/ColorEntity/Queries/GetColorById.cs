using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.ColorEntity.Queries.GetColorById;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.ColorEntity.Queries;

public class GetColorById
{
    private readonly Fixture _fixture;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly GetColorByIdQuery _query;
    private readonly GetColorByIdQueryHandler _handler;

    public GetColorById()
    {
        _fixture = new Fixture();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _query = _fixture.Create<GetColorByIdQuery>();
        _handler = new GetColorByIdQueryHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task GetColorById_ShouldBe_Success()
    {
        var color = _fixture.Create<Color>();

        _mockUnitOfWork
            .Setup(x => x.ColorRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(color);

        var result = await _handler.Handle(_query, default);

        _mockUnitOfWork.Verify(x => x.ColorRepository.GetByIdAsync(It.IsAny<int>()), Times.Once());

        result.Id.Should().Be(color.Id);
    }

    [Fact]
    public async Task GetColorById_ShouldBe_ColorNotFoundException()
    {
        _mockUnitOfWork
            .Setup(x => x.ColorRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Color?)null);

        Func<Task> act = async () => await _handler.Handle(_query, default);

        await act.Should().ThrowAsync<ColorNotFoundException>();
    }
}
