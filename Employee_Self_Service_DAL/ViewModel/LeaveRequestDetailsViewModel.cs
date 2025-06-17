namespace Employee_Self_Service_DAL.ViewModel;

public class LeaveRequestDetailsViewModel
{
    public int LeaveRequestId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal ActualDuration { get; set; }
    public decimal TotalDuration { get; set; }
    public DateOnly ReturnDate { get; set; }
    public bool? AvailableOnPhone { get; set; }
    public DateTime ApprovedDate { get; set; }
    public string Status { get; set; }
    public bool? AdhocLeave { get; set; }
    public DateOnly Date {get;set;}
}
