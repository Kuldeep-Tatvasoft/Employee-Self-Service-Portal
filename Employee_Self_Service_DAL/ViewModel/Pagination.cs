namespace Employee_Self_Service_DAL.ViewModel;

public class Pagination
{
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public int TotalRecord { get; set; }
    public int FromRec { get; set; }
    public int ToRec { get; set; }
    public int PageSize { get; set; }
}

