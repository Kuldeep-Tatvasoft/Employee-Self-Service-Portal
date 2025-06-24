using System.ComponentModel.DataAnnotations;

namespace Employee_Self_Service_DAL.ViewModel;

public class LoginViewModel
{   
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "New Password is Required")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[\W_]).{6,14}$", ErrorMessage = "Password must contain at least one letter, one number, and one special character")]
    public string NewPassword { get; set; } = null!;

    [Required (ErrorMessage = "Confirm Password is Required")]
    [Compare("NewPassword", ErrorMessage ="Confirm Password does not match")]
    public string ConfirmPassword { get; set; }
    public bool RememberMe { get; set; }    
}

