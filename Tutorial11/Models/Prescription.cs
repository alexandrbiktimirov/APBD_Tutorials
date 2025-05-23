using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tutorial11.Models;

public class Prescription
{
    [Key]
    public int IdPrescription { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    [ForeignKey(nameof(Patient))]
    public int IdPatient { get; set; }
    public Patient Patient { get; set; }

    [ForeignKey(nameof(Doctor))]
    public int IdDoctor { get; set; }
    public Doctor Doctor { get; set; }

    public ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
}