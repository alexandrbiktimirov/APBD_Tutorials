using System.ComponentModel.DataAnnotations;

namespace Tutorial11.Models;

public class Medicament
{
    public Medicament()
    {
    }

    public Medicament(int idMedicament, string description, string name, string type)
    {
        IdMedicament = idMedicament;
        Description = description;
        Name = name;
        Type = type;
    }

    [Key]
    public int IdMedicament { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    [Required]
    public string Type { get; set; }
}