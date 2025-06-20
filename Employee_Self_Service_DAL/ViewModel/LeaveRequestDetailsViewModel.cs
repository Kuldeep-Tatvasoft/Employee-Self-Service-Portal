namespace Employee_Self_Service_DAL.ViewModel;

public class LeaveRequestDetailsViewModel
{
    public int LeaveRequestId { get; set; }
    public int EmployeeId {get; set;}
    public string EmployeeEmail {get; set;}
    public string EmployeeName { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string ReasonName{ get; set;}
    public decimal ActualDuration { get; set; }
    public decimal TotalDuration { get; set; }
    public DateOnly ReturnDate { get; set; }
    public bool? AvailableOnPhone { get; set; }
    // public DateTime ApprovedDate { get; set; }
    public string ApprovedDate { get; set;}
    public string ApprovedBy {get ; set;}
    public int StatusId { get; set;}
    public string Status { get; set; }
    public bool AdhocLeave { get; set; }
    public DateOnly Date {get;set;}
}
