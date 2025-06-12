namespace Employee_Self_Service_DAL.ViewModel;

public class AddLeaveRequestViewModel
{
    public int LeaveRequestId { get; set; }
    public string Name { get; set; }
    public string ReportingPerson {get; set;}
    public string LeaveReason {get; set;}
    public string ReasonDescriptin {get; set;}
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int ActualDuration { get; set; }
    public int TotalDuration { get; set; }
    public DateTime ReturnDate { get; set; }
    public bool? AvailableOnPhone { get; set; }
    public DateOnly RequestedDate {get; set;}
    public string PhoneNo {get; set;} 
    public string AlternatePhoneNo {get; set;} 
    public DateTime ApprovedDate { get; set; }
    public string Status { get; set; }
    public bool? AdhocLeave { get; set; }
}
