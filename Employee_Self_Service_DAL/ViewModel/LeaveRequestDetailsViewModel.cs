using Employee_Self_Service.DAL.Attributes;

namespace Employee_Self_Service_DAL.ViewModel;

public class LeaveRequestDetailsViewModel
{
    public int LeaveRequestId { get; set; }
    public int EmployeeId {get; set;}
    public string EmployeeEmail {get; set;}
    [DisplayColumn("Employee Name")]
    public string EmployeeName { get; set; }
    [DisplayColumn("Start Date")]
    public DateOnly StartDate { get; set; }
    [DisplayColumn("End Date")]
    public DateOnly EndDate { get; set; }
    [DisplayColumn("Reason Name")]
    public string ReasonName{ get; set;}
    [DisplayColumn("Return Date")]
    public DateOnly ReturnDate { get; set; }
    [DisplayColumn("Actual Duration")]
    public decimal ActualDuration { get; set; }
    [DisplayColumn("Approved By")]
    public string ApprovedBy {get ; set;}
   
    [DisplayColumn("Approved Date")]
    public string ApprovedDate { get; set;}    
    public int StatusId { get; set;}
    [DisplayColumn("Status")]
    public string Status { get; set; }
    [DisplayColumn("Adhoc Leave")]
    public bool AdhocLeave { get; set; }
     public decimal TotalDuration { get; set; }
     public bool? AvailableOnPhone { get; set; }
    public DateOnly Date {get;set;}
}
