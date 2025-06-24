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

    public async Task<EventPaginationViewModel> GetEventData(int pageSize, int pageNumber, string searchQuery, string sortColumn, string sortDirection, string eventFromDate, string eventToDate, string eventCategory)
    {
        try
        {
            var List = await _eventRepository.GetPaginatedEvent(pageSize, pageNumber, searchQuery, sortColumn, sortDirection, eventFromDate, eventToDate, eventCategory);
            return List;
        }
        catch (Exception ex)
        {
            throw;
        }
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

    public async Task<AddEventViewModel> EventDetails(long eventId)
    {
       
            Event eventDetails = await _eventRepository.GetEventDetails(eventId);
            if(eventDetails == null)
            {
                return null;
            }
            AddEventViewModel model = new AddEventViewModel
            {
                EventId = eventDetails.EventId,
                EventName = eventDetails.Name,
                EventDescription = eventDetails.Description,
                StartDate = (DateOnly)eventDetails.StartDate,
                EndDate = (DateOnly)eventDetails.EndDate,
                EventDocuments = await _eventRepository.GetEventDocuments(eventId)
            };
            return model;
        
    }
}
