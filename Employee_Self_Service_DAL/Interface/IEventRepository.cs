using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.AspNetCore.Http;

namespace Employee_Self_Service_DAL.Interface;

public interface IEventRepository
{   
    Task<EventPaginationViewModel> GetPaginatedEvent(int pageSize, int pageNumber, string searchQuery, string sortColumn, string sortDirection, string eventFromDate, string eventToDate, string eventCategory);
    Task<List<EventCategory>> GetCategories();
    Task<ResponseViewModel> AddEvent(Event newEvent, List<IFormFile> Documents);
    Task<ResponseViewModel> AddNotification (Notification addNotification);
    Task<Event> GetEventDetails(long eventId);
    // Task<List<Document>> GetEventDocuments(long eventId);
    Task<ResponseViewModel> EditEvent(Event update,List<IFormFile> Documents);
    
}
