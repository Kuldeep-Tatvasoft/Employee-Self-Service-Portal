using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Employee_Self_Service_DAL.ViewModel;

public class ProfileViewModel
{
    public int EmployeeId { get; set;}
    public string? ProfileImage {get; set;}
    // public IFormFile? ProfileImage2 { get; set; }
    public IFormFile? FormFile { get; set; }
    public string? Name { get; set;}
    public string Email { get; set; } = null!;
    public string DateOfBirth { get; set;}
    public string? Designation {get; set;}
    public DateOnly StartingDate {get; set;}
    public string Gender { get; set;} 
    public decimal Experience { get; set;}
    public string? Department {get; set;}
    public int CardNumber { get; set;}
    public string SeatingLocation {get; set;}
    public string ReportingPerson {get; set;}
    public string BloodGroup { get; set;} = null;
    public string ProjectManager {get; set;}
    public string AnyDiseases {get; set;} = null;
    [Required(ErrorMessage = "Emergency Contact Required")]
    public string? ContactNo {get; set;}
}
