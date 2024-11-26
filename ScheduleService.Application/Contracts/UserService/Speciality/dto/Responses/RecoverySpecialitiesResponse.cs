namespace ScheduleService.Application.Contracts.UserService.Speciality.dto.Responses;

public record RecoverySpecialitiesResponse
{
    public List<SpecialityShortInfo> Specialities { get; set; }
}
