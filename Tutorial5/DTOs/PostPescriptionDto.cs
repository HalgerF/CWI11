using System.ComponentModel.DataAnnotations;

namespace Tutorial5.DTOs;

public class PostPrescriptionDto : IValidatableObject
{
    [Required]
    public PatientDto Patient { get; set; }

    [Required]
    [MaxLength(10, ErrorMessage = "Maksymalnie 10 leków na recepcie.")]
    public ICollection<PrescriptionMedicamentDto> Medicaments { get; set; }

    [Required]
    public DateTime? Date { get; set; }

    [Required]
    public DateTime? DueDate { get; set; }

    [Required]
    public int? IdDoctor { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Date.HasValue && DueDate.HasValue && DueDate < Date)
        {
            yield return new ValidationResult(
                "DueDate musi być większa lub równa Date.",
                new[] { nameof(DueDate) });
        }
    }
}