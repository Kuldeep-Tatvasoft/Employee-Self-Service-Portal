namespace Employee_Self_Service_DAL.ViewModel;

public class LeaveRequestDetailsViewModel
{
    public int LeaveRequestId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int ActualDuration { get; set; }
    public int TotalDuration { get; set; }
    public DateTime? ReturnDate { get; set; }
    public bool? AvailableOnPhone { get; set; }
    public DateTime ApprovedDate { get; set; }
    public string Status { get; set; }
    public bool? AdhocLeave { get; set; }
    public DateOnly Date {get;set;}
}
