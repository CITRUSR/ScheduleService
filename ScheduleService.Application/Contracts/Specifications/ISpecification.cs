namespace ScheduleService.Application.Contracts.Specifications;

public interface ISpecification<T>
{
    string WhereClause { get; }
}
