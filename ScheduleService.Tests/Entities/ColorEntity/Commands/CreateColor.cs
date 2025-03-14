using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.Contracts.Helpers;
using ScheduleService.Application.CQRS.ColorEntity.Commands.CreateColor;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.ColorEntity.Commands;

public class CreateColor
{
    private readonly Fixture _fixture;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IUniqueConstraintExceptionChecker> _mockChecker;
    private readonly CreateColorCommandHandler _handler;
    private readonly CreateColorCommand _command;

    public CreateColor()
    {
        _fixture = new Fixture();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockChecker = new Mock<IUniqueConstraintExceptionChecker>();
        _handler = new CreateColorCommandHandler(_mockUnitOfWork.Object, _mockChecker.Object);
        _command = _fixture.Create<CreateColorCommand>();
    }

    [Fact]
    public async Task CreateCommand_ShouldBe_Success()
    {
        _mockUnitOfWork
            .Setup(x => x.ColorRepository.InsertAsync(It.IsAny<Color>()))
            .ReturnsAsync(new Color());

        var result = await _handler.Handle(_command, default);

        _mockUnitOfWork.Verify(x => x.ColorRepository.InsertAsync(It.IsAny<Color>()), Times.Once());
        _mockUnitOfWork.Verify(x => x.CommitTransaction(), Times.Once());

        result.Id.Should().Be(0);
    }

    [Fact]
    public async Task CreateColor_ShouldBe_ColorNameAlreadyExistsException()
    {
        var ex = new Exception("UniqueEx");

        _mockUnitOfWork.Setup(x => x.ColorRepository.InsertAsync(It.IsAny<Color>())).Throws(ex);
        _mockChecker.Setup(x => x.Check<Color>(ex)).Returns("Name");

        Func<Task> act = async () => await _handler.Handle(_command, default);

        await act.Should().ThrowAsync<ColorNameAlreadyExistsException>();

        _mockUnitOfWork.Verify(x => x.ColorRepository.InsertAsync(It.IsAny<Color>()), Times.Once());
        _mockChecker.Verify(x => x.Check<Color>(ex), Times.Once());
        _mockUnitOfWork.Verify(x => x.CommitTransaction(), Times.Never());
    }
}
