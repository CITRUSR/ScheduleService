namespace ScheduleService.Application.Contracts.UserService.Teacher.dto.responses;

public class TeacherViewModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? PatronymicName { get; set; }
}
