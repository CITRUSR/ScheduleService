using AutoFixture;
using FluentAssertions;
using ScheduleService.Application.Common.Extensions;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.extensions;

public class ClassExtensions
{
    private readonly Fixture _fixture;

    public ClassExtensions()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void ToColorClasses_ShouldBe_Success()
    {
        var classesWithBlue = _fixture
            .Build<Class>()
            .With(x => x.Id, 1)
            .With(
                x => x.Color,
                _fixture.Build<Color>().With(c => c.Id, 1).With(c => c.Name, "Blue").Create()
            )
            .CreateMany(4)
            .ToList();

        var classesWithRed = _fixture
            .Build<Class>()
            .With(x => x.Id, 2)
            .With(
                x => x.Color,
                _fixture.Build<Color>().With(c => c.Id, 2).With(c => c.Name, "Red").Create()
            )
            .CreateMany(3)
            .ToList();

        var resultClasses = classesWithBlue
            .Concat(classesWithRed)
            .ToList()
            .ToColorClasses<ClassDetailBase>();

        resultClasses.Count.Should().Be(2);
        resultClasses.FirstOrDefault(x => x.Color.Id == 1).Classes.Count.Should().Be(4);
        resultClasses.FirstOrDefault(x => x.Color.Id == 2).Classes.Count.Should().Be(3);
        resultClasses.FirstOrDefault(x => x.Color.Id == 1).Classes.All(x => x.Id == 1);
        resultClasses.FirstOrDefault(x => x.Color.Id == 2).Classes.All(x => x.Id == 2);
    }

    [Fact]
    public void ToWeekdayColorClasses_ShouldBe_Success()
    {
        var classesWithMon = _fixture
            .Build<Class>()
            .With(
                x => x.Weekday,
                _fixture.Build<Weekday>().With(c => c.Id, 1).With(c => c.Name, "Mon").Create()
            )
            .CreateMany(3);

        var classesWithTue = _fixture
            .Build<Class>()
            .With(
                x => x.Weekday,
                _fixture.Build<Weekday>().With(c => c.Id, 2).With(c => c.Name, "Tue").Create()
            )
            .CreateMany(4);

        var weekdayClasses = classesWithMon
            .Concat(classesWithTue)
            .ToList()
            .ToWeekdayColorClasses<ColorClassesDto<ClassDetailBase>, ClassDetailBase>();

        weekdayClasses.Count.Should().Be(2);
        weekdayClasses.FirstOrDefault(x => x.Weekday.Id == 1).Classes.Count.Should().Be(3);
        weekdayClasses.FirstOrDefault(x => x.Weekday.Id == 2).Classes.Count.Should().Be(4);
        weekdayClasses
            .FirstOrDefault(x => x.Weekday.Id == 1)
            .Classes.Select(x => x.Classes.All(c => c.Id == 1))
            .Count()
            .Should()
            .Be(3);
        weekdayClasses
            .FirstOrDefault(x => x.Weekday.Id == 2)
            .Classes.Select(x => x.Classes.All(c => c.Id == 2))
            .Count()
            .Should()
            .Be(4);
    }
}
