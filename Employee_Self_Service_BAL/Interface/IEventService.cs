using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_BAL.Interface;

public interface IEventService
{   
    Task<EventPaginationViewModel> GetEventData(int pageSize, int pageNumber, string searchQuery, string sortColumn, string sortDirection, string eventFromDate, string eventToDate, string eventCategory);
    Task<List<EventCategory>> GetCategories();
    Task<ResponseViewModel> AddEvent(AddEventViewModel model);
    Task<ResponseViewModel> AddNotification(string notification);
    Task<AddEventViewModel> EventDetails(long eventId);
    Task<ResponseViewModel> EditEvent(AddEventViewModel model);
    Task<ResponseViewModel> DeleteEvent(long eventId);
    
    // Task<List<NotificationGroupViewModel>> GetGroupedNotifications();
}
