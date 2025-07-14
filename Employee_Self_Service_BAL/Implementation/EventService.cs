using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.Interface;
using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.AspNetCore.Http;
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
    public async Task<List<EventCategory>> GetCategories()
    {
        var data = await _eventRepository.GetCategories();
        return data;
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
                CategoryId = model.CategoryId
            };

            ResponseViewModel response = await _eventRepository.AddEvent(newEvent, model.Documents);

            return response;
        }
        catch (Exception ex)
        {
            return new ResponseViewModel
            {
                success = false,
                message = "Failed to add event:" + ex.Message
            };
        }
    }

    public async Task<ResponseViewModel> AddNotification(string notification)
    {
        Notification addNotification = new Notification
        {
            Notification1 = notification,
            NotificationCategoryId = 1,

        };
        
        ResponseViewModel response = await _eventRepository.AddNotification(addNotification);
        return response;
    }

    public async Task<AddEventViewModel> EventDetails(long eventId)
    {

        Event eventDetails = await _eventRepository.GetEventDetails(eventId);
        if (eventDetails == null)
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
            CategoryId = (int)eventDetails.CategoryId,
            EventDocuments = (List<Document>)eventDetails.Documents,
            Documents = new List<IFormFile>()
        };
        return model;

    }

    public async Task<ResponseViewModel> EditEvent(AddEventViewModel model)
    {
        try
        {
            Event update = await _eventRepository.GetEventDetails(model.EventId);
            {
                update.Name = model.EventName;
                update.Description = model.EventDescription;
                update.StartDate = model.StartDate;
                update.EndDate = model.EndDate;
                update.CategoryId = model.CategoryId;
            };
            ResponseViewModel response = await _eventRepository.EditEvent(update, model.Documents);
            return response;
        }
        catch (Exception ex)
        {
            return new ResponseViewModel
            {
                success = false,
                message = "Failed to edit event:" + ex.Message
            };
        }
    }

    public async Task<ResponseViewModel> DeleteEvent(long eventId)
    {
        try
        {
            Event update = await _eventRepository.GetEventDetails(eventId);
            {
                update.IsDeleted = true;
                update.DeletedAt = DateTime.Now;
            };

            ResponseViewModel response = await _eventRepository.EditEvent(update, new List<IFormFile>());
            if (response.success)
            {
                return new ResponseViewModel
                {
                    success = response.success,
                    message = "Event Deleted Successfully"
                };
            }
            else
            {
                return new ResponseViewModel
                {
                    success = response.success,
                    message = "Error occur deleting event:" + response.message
                };
            }
        }
        catch (Exception ex)
        {
            return new ResponseViewModel
            {
                success = false,
                message = "Event Failed to Deleted:" + ex.Message
            };
        }
    }

    public async Task<byte[]> GetEventDataToExport(int pageSize, int pageNumber, string searchQuery,string eventFromDate, string eventToDate, string eventCategory)
    {
        return await _eventRepository.GetEventDataToExport(pageSize, pageNumber, searchQuery, eventFromDate, eventToDate, eventCategory);
    }
}
