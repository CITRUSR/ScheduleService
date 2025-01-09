using AutoFixture;
using FluentAssertions;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Common.Extensions;
using ScheduleService.Application.CQRS.ColorEntity.Responses;

namespace ScheduleService.Tests.extensions;

public class ColorExtension
{
    private readonly Fixture _fixture;

    public ColorExtension()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void EnsureColorExists_ShouldBe_Success()
    {
        var colors = _fixture.Build<ColorViewModel>().With(x => x.Name, "Red").CreateMany(4);

        Action act = () => colors.EnsureColorExists("Blue");

        act.Should().Throw<ColorNotFoundException>();
    }
}
