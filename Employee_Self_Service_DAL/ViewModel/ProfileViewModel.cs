namespace Employee_Self_Service_DAL.ViewModel;

public class ProfileViewModel
{
    public string? Name { get; set;}
    public string Email { get; set; } = null!;
    public DateOnly? DateOfBirth { get; set;}
    public string? Designation {get; set;}
    public DateOnly StartingDate {get; set;}
    public string Gender { get; set;} 
    public decimal Experience { get; set;}
    public string? Department {get; set;}
    public int CardNumber { get; set;}
    public string SeatingLocation {get; set;}
    public string ReportingPerson {get; set;}
    public string BloodGroup { get; set;}
    public string ProjectManager {get; set;}
    public string AnyDiseases {get; set;}
    public string ContactNo {get; set;}
}
