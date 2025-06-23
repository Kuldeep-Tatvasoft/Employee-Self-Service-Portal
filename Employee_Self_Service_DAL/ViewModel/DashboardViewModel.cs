namespace Employee_Self_Service_DAL.ViewModel;

public class DashboardViewModel
{
    public List<LeaveRequestDetailsViewModel> TodayOnLeave {get; set;} = new List<LeaveRequestDetailsViewModel>();
    public List<AddEventViewModel> UpcomingEvents {get; set;} = new List<AddEventViewModel>();
}
