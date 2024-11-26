namespace ScheduleService.Application.Contracts.UserService.Speciality.dto.Requests;

public record SoftDeleteSpecialitiesRequest
{
    public List<int> Ids { get; set; }
}
