using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Common.Exceptions.CurrentWeekdayEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.CurrentWeekdayEntity.Commands.UpdateCurrentWeekday;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.CurrentWeekdayEntity.Commands;

public class UpdateCurrentWeekday
{
    private readonly Fixture _fixture;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly UpdateCurrentWeekdayCommandHandler _handler;
    private readonly UpdateCurrentWeekdayCommand _command;

    public UpdateCurrentWeekday()
    {
        _fixture = new Fixture();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new UpdateCurrentWeekdayCommandHandler(_mockUnitOfWork.Object);
        _command = _fixture.Build<UpdateCurrentWeekdayCommand>().With(x => x.Color, "Red").Create();
    }

    [Fact]
    public async Task UpdateCurrentWeekday_ShouldBe_Success()
    {
        _mockUnitOfWork
            .Setup(x => x.CurrentWeekdayRepository.UpdateAsync(It.IsAny<CurrentWeekday>()))
            .ReturnsAsync(new CurrentWeekday());

        var result = await _handler.Handle(_command, default);

        _mockUnitOfWork.Verify(
            x => x.CurrentWeekdayRepository.UpdateAsync(It.IsAny<CurrentWeekday>()),
            Times.Once()
        );

        _mockUnitOfWork.Verify(x => x.CommitTransaction(), Times.Once());

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateCurrentWeekday_ShouldBe_CurrentWeekdayNotFoundException()
    {
        _mockUnitOfWork
            .Setup(x => x.CurrentWeekdayRepository.UpdateAsync(It.IsAny<CurrentWeekday>()))
            .ReturnsAsync((CurrentWeekday?)null);

        Func<Task> act = async () => await _handler.Handle(_command, default);

        await act.Should().ThrowAsync<CurrentWeekdayNotFoundException>();
    }
}
