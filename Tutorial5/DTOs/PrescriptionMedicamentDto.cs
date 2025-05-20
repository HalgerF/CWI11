using System.ComponentModel.DataAnnotations;

namespace Tutorial5.DTOs;
public class PrescriptionMedicamentDto
{
    [Required]
    public int IdMedicament { get; set; }

    public int? Dose { get; set; }

    [Required]
    [MaxLength(100)]
    [MinLength(1, ErrorMessage = "Details nie może być puste.")]
    public string Details { get; set; }
}