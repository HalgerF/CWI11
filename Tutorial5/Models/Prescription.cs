using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tutorial5.Models;

public class Prescription
{
    [Key]
    public int IdPrescription { get; set; }

    [Required]
    public DateOnly Date { get; set; }

    [Required]
    public DateOnly DueDate { get; set; }

    [ForeignKey(nameof(Patient))]
    public int IdPatient { get; set; }

    [ForeignKey(nameof(Doctor))]
    public int IdDoctor { get; set; }

    public Patient Patient { get; set; }
    public Doctor Doctor { get; set; }

    public ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; } = new List<PrescriptionMedicament>();
}