using Tutorial5.DTOs;
using System.Threading.Tasks;

namespace Tutorial5.Services;

public interface IPrescriptionService
{
    Task<int> CreatePrescriptionAsync(PostPrescriptionDto prescription);
    Task<PatientDetailsDto> GetPatientAsync(int id);
}