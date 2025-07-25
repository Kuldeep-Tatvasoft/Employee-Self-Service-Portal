using System.Drawing.Imaging;
using Employee_Self_Service_DAL.Data;
using Employee_Self_Service_DAL.Interface;
using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Employee_Self_Service_DAL.Implementation;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly EmployeeSelfServiceContext _context;

    public EmployeeRepository(EmployeeSelfServiceContext context)
    {
        _context = context;
    }
    public Employee GetUserByEmail(string email)
    {
        return _context.Employees.Include(u => u.Role).FirstOrDefault(u => u.Email == email);
    }

    public Employee GetEmployeeById(int employeeId)
    {
        return _context.Employees.Include(u => u.Role).FirstOrDefault(u => u.EmployeeId == employeeId);
    }



    public async Task<ResponseViewModel> RegisterEmployee(Employee employee)
    {
        try
        {
            _context.Add(employee);
            await _context.SaveChangesAsync();
            return new ResponseViewModel
            {
                success = true,
                message = "Registration Successfully"
            };
        }
        catch (Exception ex)
        {
            return new ResponseViewModel
            {
                success = false,
                message = "Error occur Register User" + ex.Message
            };

        }
    }

    public async Task<ResponseViewModel> UpdateEmployee(Employee employee)
    {
        try
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return new ResponseViewModel
            {
                success = true
            };
        }
        catch (Exception ex)
        {
            return new ResponseViewModel
            {
                success = false,
                message = ex.Message
            };
        }
    }

    public async Task<List<WidgetMapping>> GetWidgets(int employeeId)
    {
        return await _context.WidgetMappings
            .Include(wm => wm.Widget)
            .Where(wm => wm.EmployeeId == employeeId)
            .OrderBy(wm => wm.Widget.WidgetId)
            .ToListAsync();
    }

    // public async Task<Widget> GetWidgetById(long widgetId)
    // {
    //     return await _context.Widgets.FirstOrDefaultAsync(w => w.WidgetId == widgetId);
    // }

    public async Task<ResponseViewModel> UpdateWidget(WidgetMapping widget)
    {
        try
        {
            _context.WidgetMappings.Update(widget);
            await _context.SaveChangesAsync();
            return new ResponseViewModel
            {
                success = true,
            };
        }
        catch (Exception ex)
        {
            return new ResponseViewModel
            {
                success = false,
                message = ex.Message
            };
        }
    }

    public async Task<List<QuickLink>> GetQuickLinks()
    {
        return await _context.QuickLinks.OrderBy(w => w.QuickLinkId).Where(q => q.IsDeleted == false).ToListAsync();
    }

    public async Task<List<QuickLinkViewModel>> GetQuickLink(long employeeId)
    {
        return await _context.QuickLinks
            .OrderBy(q => q.QuickLinkId)
            .Where(q => q.IsDeleted == false && q.EmployeeId == employeeId)
            .Select(q => new QuickLinkViewModel
            {
                QuickLinkId = q.QuickLinkId,
                Name = q.Name,
                Url = q.Url,
                IsDeleted = (bool)q.IsDeleted
            }).ToListAsync();
    }

    public async Task<ResponseViewModel> AddQuickLink(List<QuickLinkViewModel> links,int employeeId)
    {
        try
        {
            foreach (var link in links)
            {
                if (link.QuickLinkId > 0)
                {
                    var existing = _context.QuickLinks.FirstOrDefault(q => q.QuickLinkId == link.QuickLinkId);
                    if (existing != null)
                    {
                        existing.Name = link.Name;
                        existing.Url = link.Url;
                        _context.QuickLinks.Update(existing);
                    }
                }
                else
                {
                    var newLink = new QuickLink
                    {
                        Name = link.Name,
                        Url = link.Url,
                        EmployeeId = employeeId
                    };
                    _context.QuickLinks.Add(newLink);
                }
            }
            await _context.SaveChangesAsync();
            return new ResponseViewModel
            {
                success = true,
                message = "Quick Links updated successfully."
            };
        }
        catch (Exception ex)
        {
            return new ResponseViewModel
            {
                success = false,
                message = ex.Message
            };
        }
    }

    public async Task<ResponseViewModel> UpdateQuickLink(QuickLink quickLink)
    {
        try
        {
            _context.QuickLinks.Update(quickLink);
            await _context.SaveChangesAsync();
            return new ResponseViewModel
            {
                success = true
            };
        }
        catch (Exception ex)
        {
            return new ResponseViewModel
            {
                success = false,
                message = ex.Message
            };
        }
    }

}


