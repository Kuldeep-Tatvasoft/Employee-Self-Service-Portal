using Employee_Self_Service.DAL.Attributes;
using Employee_Self_Service_DAL.Models;

namespace Employee_Self_Service_DAL.ViewModel;

public class EventDetailsViewModel
{
    public long EventId { get; set; } 
    [DisplayColumn("Event Name")]    
    public string EventName { get; set; }
    public string EventDescription { get; set; }    
    [DisplayColumn("Start Date")]
    public DateOnly StartDate { get; set; }
    [DisplayColumn("End Date")]
    public DateOnly EndDate { get; set; }    
    public int CategoryId { get; set; }
    [DisplayColumn("Category Name")]
    public string CategoryName { get; set;}    
    
    // public List<IFormFile> Documents { get; set; } = new List<IFormFile>();

    public List<Document> EventDocuments { get; set;} = new List<Document>();

    public List<EventCategory> Categories { get; set;} = new List<EventCategory>();   
    
}