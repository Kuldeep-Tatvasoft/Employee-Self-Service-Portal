// using static Employee_Self_Service_DAL.Constants.HelpDeskEnum;

// namespace Employee_Self_Service_DAL.ViewModel;

// public class HelpDeskDetailsViewModel
// {
//     public int HelpDeskRequestId {get; set;}
//     public string Group {get; set;}
//     public string Category { get; set; }
//     public string SubCategory { get; set; }
//     public Priority Priority {get; set;}
//     public string ServiceDetails { get; set; }
//     public string Status { get; set; }
// }


// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations;
using Employee_Self_Service.DAL.Attributes;
using static Employee_Self_Service_DAL.Constants.HelpDeskEnum;



namespace Employee_Self_Service_DAL.ViewModel
{
    public class HelpDeskDetailsViewModel
    {
        [DisplayColumn("Ticket ID")]
        public long HelpDeskRequestId { get; set; }

        [DisplayColumn("Request DateTime")]
        public DateTime RequestedDate { get; set; }

        [DisplayColumn("Group")]
        public string Group { get; set; }

        [DisplayColumn("Category")]
        public string Category { get; set; }
        [DisplayColumn("Sub Category")]
        public List<string> SubCategories { get; set; }
        
        [DisplayColumn("Priority")]
        public Priority Priority { get; set; }

        [DisplayColumn("Service Details")]
        public string ServiceDetails { get; set; }

        [DisplayColumn("Pending At")]
        public string PendingAt { get; set; }

        [DisplayColumn("Status")]
        public string? Status { get; set; }

        public int? StatusId { get; set; }
        public string SubCategory { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeId { get; set; }
        public int GroupId { get; set; }
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public string Comment { get; set; }
        
        public int[] selectedSubCategories { get; set; } = Array.Empty<int>();
        public string InsertedBy { get; set; }
        public int historyStatus { get; set; }
    }
}
