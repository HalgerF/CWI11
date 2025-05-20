using System.ComponentModel.DataAnnotations;

namespace Tutorial5.DTOs;

public class DoctorDto
{
    public int IdDoctor { get; set; }
    [MaxLength(100)] 
    public string FirstName { get; set; }
    [MaxLength(100)]
    public string LastName { get; set; }
    [MaxLength(100)] 
    public string Email { get; set; }
}