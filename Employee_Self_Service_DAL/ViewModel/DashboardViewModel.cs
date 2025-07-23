using Employee_Self_Service_DAL.Models;

namespace Employee_Self_Service_DAL.ViewModel;

public class DashboardViewModel
{
    public List<LeaveRequestDetailsViewModel> TodayOnLeave {get; set;} = new List<LeaveRequestDetailsViewModel>();
    public List<AddEventViewModel> UpcomingEvents {get; set;} = new List<AddEventViewModel>();
    public List<LeaveRequestDetailsViewModel> OnLeave {get; set;} = new List<LeaveRequestDetailsViewModel>();
    public List<HelpDeskDetailsViewModel> OwnHelpDeskRequests {get; set;} = new List<HelpDeskDetailsViewModel>();
    public List<Widget> Widgets { get; set;} = new List<Widget>();
    public List<QuickLinkViewModel> QuickLinks {get; set;} = new List<QuickLinkViewModel>();
}
