namespace Tutorial11.Models.DTOs;

public class PrescriptionDto
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<MedicamentPatientDto> Medicaments { get; set; }
    public DoctorDto Doctor { get; set; }
}