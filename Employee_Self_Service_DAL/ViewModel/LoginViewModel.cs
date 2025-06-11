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
    [RegularExpression("([a-z]|[A-Z]|[0-9]|[\\W]){4}[a-zA-Z0-9\\W]{3,11}", ErrorMessage = "Password Must contain Special Symbol, Number,Alphabet")]
    public string NewPassword { get; set; } = null!;

    [Required (ErrorMessage = "Confirm Password is Required")]
    [Compare("NewPassword", ErrorMessage ="Confirm Password does not match")]
    public string ConfirmPassword { get; set; }
    public bool RememberMe { get; set; }    
}

