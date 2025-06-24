using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Employee_Self_Service_DAL.ViewModel;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Name Is Required")]
    public string? Name { get; set;}

    [Required (ErrorMessage = "Role is Required")]
    public int RoleId { get; set;}

    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is Required")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[\W_]).{6,14}$", ErrorMessage = "Password must contain at least one letter, one number, and one special character")]
    public string Password { get; set; } = null!;

    [Required (ErrorMessage = "Confirm Password is Required")]
    [Compare("Password", ErrorMessage ="Confirm Password does not match")]
    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "Designation is Required")]
    public string? Designation {get; set;}


    [Required (ErrorMessage = "Contact No is Required")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
    public string Phone {get; set;}

    [Required(ErrorMessage = "Department is Required")]
    public string? Department { get; set;}
    
    public IFormFile? ProfileImage { get; set; }
}
