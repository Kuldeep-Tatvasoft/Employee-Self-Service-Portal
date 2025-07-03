using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using Employee_Self_Service_DAL.Models;

namespace Employee_Self_Service_DAL.ViewModel;

public class AddLeaveRequestViewModel
{   
    public int EmployeeId {get; set;}
    public string EmployeeEmail {get; set;}
    public int LeaveRequestId { get; set; }
    public string Name { get; set; }
    public string ReportingPerson {get; set;}
    [Required(ErrorMessage = "Reason is required")]
    public int ReasonId {get; set;}
    public string ReasonName{ get; set;}
    [Required(ErrorMessage ="Reason Description is Required")]
    public string ReasonDescription {get; set;}
    [Required (ErrorMessage ="Start Date is Required")]
    public DateOnly? StartDate { get; set; }   
    public int StartLeaveTypeId {get; set;}
    public string StartLeaveType {get; set;}
    [Required (ErrorMessage ="End Date is Required")]
    public DateOnly? EndDate { get; set; }
    public int EndLeaveTypeId {get; set;}
    public string EndLeaveType {get; set;}
    [Required(ErrorMessage ="Actual Duration is Required")]
    [Range(0.5, double.MaxValue , ErrorMessage = "Actual Duration must be at least 0.5")]    
    public decimal? ActualDuration { get; set; }
    [Required(ErrorMessage ="Total Duration is Required")]
    [Range(0.5, double.MaxValue, ErrorMessage = "Total Duration must be at least 0.5")]
    public decimal? TotalDuration { get; set; }
    public DateOnly? ReturnDate { get; set; }
    
    [Required]
    public bool? AvailableOnPhone { get; set; }
    [Required]
    public DateOnly RequestedDate {get; set;}
    [Required (ErrorMessage = "Phone No is Required")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
    public string PhoneNo {get; set;} 
    
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
    public string AlternatePhoneNo {get; set;} 
    [Required]
    public bool AdhocLeave { get; set; }
    public ProfileViewModel profile { get; set; }
    public virtual Reason? ReasonNavigation { get; set; }
    public List<Reason> Reasons { get; set;} = new List<Reason>();
    [Required (ErrorMessage = "Comment is Required")]
    public string Comment { get; set;}
    public int StatusId {get; set;}
    public string LeaveStatus  { get; set; }
    public string ApprovedBy {get;set;}
}
