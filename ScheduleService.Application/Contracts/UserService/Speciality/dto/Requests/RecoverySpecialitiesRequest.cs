namespace ScheduleService.Application.Contracts.UserService.Speciality.dto.Requests;

public record RecoverySpecialitiesRequest
{
    public List<int> Ids { get; set; }
}
