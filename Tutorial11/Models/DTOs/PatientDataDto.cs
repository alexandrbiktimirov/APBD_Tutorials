using Tutorial11.DTOs;

namespace Tutorial11.Models.DTOs;

public class PatientDataDto
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public List<PrescriptionDto> Prescriptions { get; set; }
}