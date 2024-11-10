using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.ColorEntity.Commands.DeleteColor;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.ColorEntity.Commands;

public class DeleteColor
{
    private readonly Fixture _fixture;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly DeleteColorCommand _command;
    private readonly DeleteColorCommandHandler _handler;

    public DeleteColor()
    {
        _fixture = new Fixture();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _command = _fixture.Create<DeleteColorCommand>();
        _handler = new DeleteColorCommandHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task DeleteColor_ShouldBe_Success()
    {
        _mockUnitOfWork
            .Setup(x => x.ColorRepository.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync(new Color());

        await _handler.Handle(_command, default);

        _mockUnitOfWork.Verify(x => x.ColorRepository.DeleteAsync(It.IsAny<int>()), Times.Once());

        _mockUnitOfWork.Verify(x => x.CommitTransaction(), Times.Once());
    }

    [Fact]
    public async Task DeleteColor_ShouldBe_ColorNotFoundException()
    {
        _mockUnitOfWork
            .Setup(x => x.ColorRepository.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync((Color?)null);

        Func<Task> act = async () => await _handler.Handle(_command, default);

        await act.Should().ThrowAsync<ColorNotFoundException>();
    }
}
