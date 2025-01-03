using AutoFixture;
using FluentAssertions;
using ScheduleService.Application.Common.Extensions;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses;

namespace ScheduleService.Tests.extensions;

public class ColorClassesDtoExtension
{
    private readonly Fixture _fixture;

    public ColorClassesDtoExtension()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void CountClassOrder_ShouldBe_Success()
    {
        var classes = _fixture
            .Build<ColorClassesDto<ClassDetailBase>>()
            .With(
                x => x.Classes,
                _fixture.Build<ClassDetailBase>().With(c => c.Order, 0).CreateMany(5).ToList()
            )
            .CreateMany(3)
            .ToList();

        classes.CountClassOrder();

        foreach (var colorClases in classes.Select(c => c.Classes))
        {
            for (int i = 0; i <= colorClases.Count - 1; i++)
            {
                var @class = colorClases[i];
                @class.Order.Should().Be(i + 1);
            }
        }
    }
}
