namespace Employee_Self_Service_DAL.ViewModel;

public class LeaveRequestPaginationViewModel
{
    public List<LeaveRequestDetailsViewModel> RequestList {get; set;} = new List<LeaveRequestDetailsViewModel>();
    public Pagination? Page { get; set; }
}
