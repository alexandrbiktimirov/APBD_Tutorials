using System.ComponentModel.DataAnnotations;

namespace Tutorial11.Models;

public class Doctor
{
    [Key]
    public int IdDoctor { get; set; }
    
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    [Required]
    public string Email { get; set; }
}