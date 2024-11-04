using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.ColorEntity.Commands.CreateColor;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.ColorEntity.Commands;

public class CreateColor
{
    private readonly Fixture _fixture;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    public CreateColor()
    {
        _fixture = new Fixture();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
    }

    [Fact]
    public async Task CreateCommand_ShouldBe_Success()
    {
        var command = _fixture.Create<CreateColorCommand>();
        var handler = new CreateColorCommandHandler(_mockUnitOfWork.Object);

        _mockUnitOfWork
            .Setup(x => x.ColorRepository.InsertAsync(It.IsAny<Color>()))
            .ReturnsAsync(new Color());

        var result = await handler.Handle(command, default);

        _mockUnitOfWork.Verify(x => x.ColorRepository.InsertAsync(It.IsAny<Color>()), Times.Once());
        _mockUnitOfWork.Verify(x => x.CommitTransaction());

        result.Id.Should().Be(0);
    }
}
