namespace ScheduleService.Application.Contracts.UserService.Speciality.dto.Responses;

public record SpecialityDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Abbreviation { get; set; }
    public double Cost { get; set; }
    public byte DurationMonths { get; set; }
    public bool IsDeleted { get; set; }
}
