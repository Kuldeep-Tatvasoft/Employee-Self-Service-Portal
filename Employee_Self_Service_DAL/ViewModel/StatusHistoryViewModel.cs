namespace Employee_Self_Service_DAL.ViewModel;

public class StatusHistoryViewModel
{   
    public long StatusHistoryId {get; set;}
    public string Name {get; set;}
    public DateTime StatusChangedDate {get; set;}
    public string Status {get; set;}
    public string Comment {get; set;}
}
