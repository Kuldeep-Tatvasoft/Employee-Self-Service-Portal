using Employee_Self_Service_DAL.Constants;

namespace Employee_Self_Service_DAL.ViewModel;

public class AddHelpDeskRequestViewModel
{
    public int HelpDeskRequestId {get; set;}
    public int GroupId {get; set;}
    public int CategoryId { get; set; }

    public int SubCategoryId { get; set; }

    public string ServiceDetails { get; set; }

    public int StatusId { get; set; }
    public Priority Priority {get; set;}
}
