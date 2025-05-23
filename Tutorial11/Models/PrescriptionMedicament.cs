using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tutorial11.Models;

public class PrescriptionMedicament
{
    public int IdMedicament { get; set; }
    public Medicament Medicament { get; set; }
    
    public int IdPrescription { get; set; }
    public Prescription Prescription { get; set; }
    
    
    public int? Dose { get; set; }
    
    [Required]
    public string Details { get; set; }
}