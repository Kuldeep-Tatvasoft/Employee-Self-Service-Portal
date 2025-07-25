using System.ComponentModel.DataAnnotations;

namespace Employee_Self_Service_DAL.ViewModel;

public class QuickLinkViewModel
{   
    public long QuickLinkId {get; set;}
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Url is required")]
    public string Url { get; set; }
    public bool IsDeleted {get; set;}
    public int EmployeeId {get; set;}
}
