using System.ComponentModel.DataAnnotations;
using Employee_Self_Service_DAL.Models;
using Microsoft.AspNetCore.Http;

namespace Employee_Self_Service_DAL.ViewModel;

public class AddEventViewModel
{   
    public long EventId { get; set; } 
    [Required(ErrorMessage = "Event Name is required")]
    public string EventName { get; set; }
    [Required(ErrorMessage = "Event Description is required")]
    public string EventDescription { get; set; }    
    [Required(ErrorMessage = "Start Date is required")]
    public DateOnly StartDate { get; set; }
    [Required(ErrorMessage = "End Date is required")]
    public DateOnly EndDate { get; set; }

    [Required (ErrorMessage = "Select a category")]
    public int CategoryId { get; set; }

    public string CategoryName { get; set;}    
    [Required(ErrorMessage = "At least one document is required")]
    public List<IFormFile> Documents { get; set; } = new List<IFormFile>();

    public List<Document> EventDocuments { get; set;} = new List<Document>();

    public List<EventCategory> Categories { get; set;} = new List<EventCategory>();

}
