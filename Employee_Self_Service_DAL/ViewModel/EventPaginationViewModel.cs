namespace Employee_Self_Service_DAL.ViewModel;

public class EventPaginationViewModel
{
    public List<AddEventViewModel> EventList {get; set;} = new List<AddEventViewModel>();
    public Pagination? Page { get; set; }
}
