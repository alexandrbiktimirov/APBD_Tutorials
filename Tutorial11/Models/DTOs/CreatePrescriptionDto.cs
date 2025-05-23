using Tutorial11.DTOs;

namespace Tutorial11.Models.DTOs;

public class CreatePrescriptionDto
{
    public PatientDto Patient { get; set; }
    public List<MedicamentDto> Medicaments { get; set; }
    public DoctorDto Doctor { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
}