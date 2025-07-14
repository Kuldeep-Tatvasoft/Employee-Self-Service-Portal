using System.ComponentModel.DataAnnotations;
using Employee_Self_Service_DAL.Constants;
using static Employee_Self_Service_DAL.Constants.HelpDeskEnum;

namespace Employee_Self_Service_DAL.ViewModel;

public class AddHelpDeskRequestViewModel
{
    public long HelpDeskRequestId { get; set; }
    [Required(ErrorMessage = "Employee is required")]
    public string EmployeeName {get; set;}
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "Group is required")]
    public int GroupId { get; set; }
    [Required(ErrorMessage = "Category is required")]
    public int CategoryId { get; set; }
    [Required(ErrorMessage = "Sub Category is required", AllowEmptyStrings = true)]
    public int? SubCategoryId { get; set; } 
    [Required(ErrorMessage = "Service Details is required")]
    public string ServiceDetails { get; set; }
    public int StatusId { get; set; }
    [Required(ErrorMessage = "Priority is required")]
    public Priority Priority { get; set; }
    public DateTime RequestedDate { get; set; }
    public string Group { get; set; }
    public string Category { get; set; }
    public string SubCategory { get; set; }
    public string PendingAt { get; set; }
    public string Status { get; set; }
    public string Comment { get; set; }

    [Required(ErrorMessage = "At least one subcategory is required")]
    [MinLength(1, ErrorMessage = "At least one subcategory must be selected.")]
    public int[] selectedSubCategories { get; set; } = Array.Empty<int>();
   
}