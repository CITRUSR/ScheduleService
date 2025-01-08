using AutoFixture;
using FluentAssertions;
using MediatR;
using Moq;
using Npgsql;
using ScheduleService.Application.Common.Exceptions.CurrentWeekdayEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.ColorEntity.Queries.GetColors;
using ScheduleService.Application.CQRS.ColorEntity.Responses;
using ScheduleService.Application.CQRS.CurrentWeekdayEntity.Commands.CreateCurrentWeekday;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.CurrentWeekdayEntity.Commands;

public class CreateCurrentWeekday
{
    private readonly Fixture _fixture;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IPublisher> _publisher;
    private readonly Mock<IMediator> _mockMediator;
    private readonly CreateCurrentWeekdayCommandHandler _handler;
    private readonly CreateCurrentWeekdayCommand _command;

    public CreateCurrentWeekday()
    {
        _fixture = new Fixture();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMediator = new Mock<IMediator>();
        _publisher = new Mock<IPublisher>();
        _handler = new CreateCurrentWeekdayCommandHandler(
            _mockUnitOfWork.Object,
            _publisher.Object,
            _mockMediator.Object
        );
        _command = _fixture.Create<CreateCurrentWeekdayCommand>();
    }

    [Fact]
    public async Task CreateCurrentWeekday_ShouldBe_Success()
    {
        var currentWeekday = _fixture.Create<CurrentWeekday>();

        _mockUnitOfWork
            .Setup(x => x.CurrentWeekdayRepository.InsertAsync(It.IsAny<CurrentWeekday>()))
            .ReturnsAsync(currentWeekday);

        SetupMediatr();

        var result = await _handler.Handle(_command, default);

        _mockMediator.Verify(
            x => x.Send(It.IsAny<GetColorsQuery>(), It.IsAny<CancellationToken>()),
            Times.Once()
        );

        _mockUnitOfWork.Verify(
            x => x.CurrentWeekdayRepository.InsertAsync(It.IsAny<CurrentWeekday>()),
            Times.Once()
        );

        _mockUnitOfWork.Verify(x => x.CommitTransaction(), Times.Once());
        _publisher.Verify(
            x => x.Publish(It.IsAny<CreateCurrentWeekdayEvent>(), It.IsAny<CancellationToken>()),
            Times.Once()
        );

        result.Id.Should().Be(currentWeekday.Id);
    }

    [Fact]
    public async Task CreateCurrentWeekday_ShouldBe_CurrentWeekdayAlreadyExistsException()
    {
        SetupMediatr();

        _mockUnitOfWork
            .Setup(x => x.CurrentWeekdayRepository.InsertAsync(It.IsAny<CurrentWeekday>()))
            .ThrowsAsync(new PostgresException("", "", "", "P0001"));

        Func<Task> act = async () => await _handler.Handle(_command, default);

        await act.Should().ThrowAsync<CurrentWeekdayAlreadyExistsException>();
    }

    private void SetupMediatr()
    {
        var color = _fixture.Build<ColorViewModel>().With(x => x.Name, _command.Color).Create();

        _mockMediator
            .Setup(x => x.Send(It.IsAny<GetColorsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([color]);
    }
}
