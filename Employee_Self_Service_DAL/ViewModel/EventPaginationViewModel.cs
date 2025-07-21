namespace Employee_Self_Service_DAL.ViewModel;

public class EventPaginationViewModel
{
    public List<EventDetailsViewModel> EventList {get; set;} = new List<EventDetailsViewModel>();
    public Pagination? Page { get; set; }
}
