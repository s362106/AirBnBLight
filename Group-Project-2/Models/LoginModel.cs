using System;
using System.ComponentModel.DataAnnotations;

namespace Group_Project_2.Models;

public class LoginModel
{
    public string Email { get; set; } = string.Empty;
    public string ProvidedPassword { get; set; } = string.Empty;
}
