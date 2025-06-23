using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.Interface;
using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Employee_Self_Service_BAL.Implementation;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;

    public EventService(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<ResponseViewModel> AddEvent(AddEventViewModel model)
    {
        try
        {
           Event newEvent = new Event
           {
                Name = model.EventName,
                Description = model.EventDescription,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
           };
           
        
            

            ResponseViewModel response = await _eventRepository.AddEvent(newEvent, model.Documents);
            return response;
        }
        catch(Exception ex)
        {   
            return new ResponseViewModel
            {
                success = false,
                message = "Failed to add event:" + ex.Message
            };
        }   
    }
}
