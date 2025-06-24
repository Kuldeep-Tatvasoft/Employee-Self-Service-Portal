using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_BAL.Interface;

public interface IEventService
{
    Task<ResponseViewModel> AddEvent(AddEventViewModel model);
    Task<AddEventViewModel> EventDetails(long eventId);
}
