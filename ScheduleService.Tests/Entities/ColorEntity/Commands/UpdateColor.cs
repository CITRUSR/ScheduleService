using AutoFixture;
using FluentAssertions;
using Moq;
using Npgsql;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.ColorEntity.Commands.UpdateColor;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.ColorEntity.Commands;

public class UpdateColor
{
    private readonly Fixture _fixture;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly UpdateColorCommand _command;
    private readonly UpdateColorCommandHandler _handler;

    public UpdateColor()
    {
        _fixture = new Fixture();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _command = _fixture.Build<UpdateColorCommand>().With(x => x.Name, "Yellow").Create();
        _handler = new UpdateColorCommandHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task UpdateColor_ShouldBe_Success()
    {
        var color = _fixture.Create<Color>();

        _mockUnitOfWork
            .Setup(x => x.ColorRepository.UpdateAsync(It.IsAny<Color>()))
            .ReturnsAsync(color);

        var result = await _handler.Handle(_command, default);

        _mockUnitOfWork.Verify(x => x.ColorRepository.UpdateAsync(It.IsAny<Color>()), Times.Once());

        _mockUnitOfWork.Verify(x => x.CommitTransaction(), Times.Once());

        result.Id.Should().Be(color.Id);
    }

    [Fact]
    public async Task UpdateColor_ShouldBe_ColorNotFoundException()
    {
        _mockUnitOfWork
            .Setup(x => x.ColorRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Color?)null);

        Func<Task> act = async () => await _handler.Handle(_command, default);

        await act.Should().ThrowAsync<ColorNotFoundException>();
    }

    [Fact]
    public async Task UpdateColor_ShouldBe_ColorNameAlreadyExistsException()
    {
        _mockUnitOfWork
            .Setup(x => x.ColorRepository.UpdateAsync(It.IsAny<Color>()))
            .ReturnsAsync(new Color());

        _mockUnitOfWork
            .Setup(x => x.ColorRepository.UpdateAsync(It.IsAny<Color>()))
            .Throws(new PostgresException("Unique", "severity", "invariantServerity", "23505"));

        Func<Task> act = async () => await _handler.Handle(_command, default);

        await act.Should().ThrowAsync<ColorNameAlreadyExistsException>();
    }
}
