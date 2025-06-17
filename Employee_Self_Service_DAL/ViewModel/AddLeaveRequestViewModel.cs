using System.ComponentModel.DataAnnotations;
using Employee_Self_Service_DAL.Models;

namespace Employee_Self_Service_DAL.ViewModel;

public class AddLeaveRequestViewModel
{   
    public int EmployeeId {get; set;}
    public int LeaveRequestId { get; set; }
    public string Name { get; set; }
    public string ReportingPerson {get; set;}
    [Required(ErrorMessage = "Reason is required")]
    public int ReasonId {get; set;}
    [Required]
    public string ReasonDescription {get; set;}
    [Required]
    public DateOnly? StartDate { get; set; }   
    public int StartLeaveTypeId {get; set;}
    [Required]
    public DateOnly? EndDate { get; set; }
    public int EndLeaveTypeId {get; set;}
    [Required(ErrorMessage ="ActualDuration is Required")]
    [Range(0.5, double.MaxValue , ErrorMessage = "ActualDuration must be at least 0.5")]    
    public decimal? ActualDuration { get; set; }
    [Required(ErrorMessage ="TotalDuration is Required")]
    [Range(0.5, double.MaxValue, ErrorMessage = "TotalDuration must be at least 0.5")]
    public decimal? TotalDuration { get; set; }
    public DateOnly? ReturnDate { get; set; }
    [Required]
    public bool? AvailableOnPhone { get; set; }
    [Required]
    public DateOnly RequestedDate {get; set;}
    [Required (ErrorMessage = "PhoneNo is Required")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
    public string PhoneNo {get; set;} 
    [Required (ErrorMessage = "AlternatePhoneNo is Required")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
    public string AlternatePhoneNo {get; set;} 
    [Required]
    public bool AdhocLeave { get; set; }
    public ProfileViewModel profile { get; set; }
    public List<Reason> Reasons { get; set;} = new List<Reason>();

}
