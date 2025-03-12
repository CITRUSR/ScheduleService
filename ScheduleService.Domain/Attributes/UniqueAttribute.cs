namespace ScheduleService.Domain.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class UniqueAttribute : Attribute
{
    public string Name { get; }

    public UniqueAttribute(string name)
    {
        Name = name;
    }
}
