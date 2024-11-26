namespace ScheduleService.Application.Contracts.UserService.Speciality.dto.Requests;

public record DeleteSpecialitiesRequest
{
    public List<int> Ids { get; set; }
}
