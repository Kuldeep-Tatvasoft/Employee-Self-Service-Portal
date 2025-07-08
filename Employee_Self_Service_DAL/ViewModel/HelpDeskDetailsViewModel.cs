using static Employee_Self_Service_DAL.Constants.HelpDeskEnum;

namespace Employee_Self_Service_DAL.ViewModel;

public class HelpDeskDetailsViewModel
{
    public int HelpDeskRequestId {get; set;}
    public string Group {get; set;}
    public string Category { get; set; }
    public string SubCategory { get; set; }
    public Priority Priority {get; set;}
    public string ServiceDetails { get; set; }
    public string Status { get; set; }
}
