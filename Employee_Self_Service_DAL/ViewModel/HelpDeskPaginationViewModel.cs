namespace Employee_Self_Service_DAL.ViewModel;

public class HelpDeskPaginationViewModel
{
    public List<AddHelpDeskRequestViewModel> HelpDeskList {get; set;} = new List<AddHelpDeskRequestViewModel>();
    public Pagination? Page { get; set; }
}
