namespace Employee_Self_Service_DAL.ViewModel;

public class EventDetailsViewModel
{
    public long EventId { get; set; } 
    
    public string? EventName { get; set; }
    
    public string? EventDescription { get; set; }    
    
    public DateOnly StartDate { get; set; }
    
    public DateOnly EndDate { get; set; }
    public int CategoryId { get; set; }

    public string CategoryName { get; set;}    
    
}