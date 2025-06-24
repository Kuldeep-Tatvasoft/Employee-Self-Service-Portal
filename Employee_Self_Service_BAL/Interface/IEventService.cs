using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_BAL.Interface;

public interface IEventService
{   
    Task<EventPaginationViewModel> GetEventData(int pageSize, int pageNumber, string searchQuery, string sortColumn, string sortDirection, string eventFromDate, string eventToDate, string eventCategory);
    Task<ResponseViewModel> AddEvent(AddEventViewModel model);
    Task<AddEventViewModel> EventDetails(long eventId);
}
