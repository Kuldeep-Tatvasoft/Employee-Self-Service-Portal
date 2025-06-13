using System.ComponentModel.DataAnnotations;
using Employee_Self_Service_DAL.Models;

namespace Employee_Self_Service_DAL.ViewModel;

public class AddLeaveRequestViewModel
{   
    public int EmployeeId {get; set;}
    public int LeaveRequestId { get; set; }
    public string Name { get; set; }
    public string ReportingPerson {get; set;}
    // public string LeaveReason {get; set;}
    [Required]
    public int ReasonId {get; set;}
    [Required]
    public string ReasonDescription {get; set;}
    [Required]
    public DateOnly StartDate { get; set; }
    public int LeaveTypeId {get; set;}
    [Required]
    public DateOnly? EndDate { get; set; }
    public int? ActualDuration { get; set; }
    public int? TotalDuration { get; set; }
    public DateOnly? ReturnDate { get; set; }
    [Required]
    public bool? AvailableOnPhone { get; set; }
    public DateOnly RequestedDate {get; set;}
    public string PhoneNo {get; set;} 
    [Required]
    public string AlternatePhoneNo {get; set;} 
    public DateTime ApprovedDate { get; set; }
    public string Status { get; set; }
    public bool AdhocLeave { get; set; }
    public ProfileViewModel profile { get; set; }
     public  Reason? ReasonNavigation { get; set; }
      public List<Reason> Reasons { get; set;} = new List<Reason>();

}
