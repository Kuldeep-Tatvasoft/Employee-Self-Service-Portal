namespace Employee_Self_Service_DAL.ViewModel;

public class HelpDeskPaginationViewModel
{
    public List<HelpDeskDetailsViewModel> HelpDeskList {get; set;} = new List<HelpDeskDetailsViewModel>();
    public Pagination? Page { get; set; }
}
