using Tutorial5.Data;
using Tutorial5.DTOs;
using Tutorial5.Models;
using Microsoft.EntityFrameworkCore;
using Tutorial5.Exceptions;

namespace Tutorial5.Services;

public class PrescriptionService(DatabaseContext context) : IPrescriptionService
{
    public async Task<int> CreatePrescriptionAsync(PostPrescriptionDto prescription)
    {
        var doctor = await GetDoctorOrThrowAsync(prescription.IdDoctor);
        var medicamentIds = prescription.Medicaments.Select(m => m.IdMedicament).ToList();
        await EnsureAllMedicamentsExistAsync(medicamentIds);

        var patientId = await GetOrCreatePatientAsync(prescription.Patient);

        var prescriptionEntity = CreatePrescriptionEntity(prescription, patientId, doctor.IdDoctor);

        await context.Prescriptions.AddAsync(prescriptionEntity);
        await context.SaveChangesAsync();

        return prescriptionEntity.IdPrescription;
    }

    public async Task<PatientDetailsDto> GetPatientAsync(int id)
    {
        var patient = await context.Patients
            .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.PrescriptionMedicaments)
                    .ThenInclude(pm => pm.Medicament)
            .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.Doctor)
            .FirstOrDefaultAsync(p => p.IdPatient == id);

        if (patient is null)
            throw new NotFoundException("Nie znaleziono pacjenta o podanym ID.");

        return new PatientDetailsDto
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Birthdate = patient.Birthdate.ToDateTime(TimeOnly.MinValue),
            Prescriptions = patient.Prescriptions.Select(pr => new PrescriptionDto
            {
                IdPrescription = pr.IdPrescription,
                Date = pr.Date.ToDateTime(TimeOnly.MinValue),
                DueDate = pr.DueDate.ToDateTime(TimeOnly.MinValue),
                Medicaments = pr.PrescriptionMedicaments.Select(pm => new MedicamentDto
                {
                    Name = pm.Medicament.Name,
                    Description = pm.Medicament.Description,
                    Type = pm.Medicament.Type,
                    Dose = pm.Dose,
                    Details = pm.Details,
                }).ToList(),
                Doctor = new DoctorDto
                {
                    IdDoctor = pr.Doctor.IdDoctor,
                    FirstName = pr.Doctor.FirstName,
                    LastName = pr.Doctor.LastName,
                    Email = pr.Doctor.Email,
                }
            }).ToList()
        };
    }

    private async Task<Doctor> GetDoctorOrThrowAsync(int? idDoctor)
    {
        var doctor = await context.Doctors.FindAsync(idDoctor);
        if (doctor is null)
            throw new NotFoundException("Lekarz o podanym ID nie istnieje.");
        return doctor;
    }

    private async Task EnsureAllMedicamentsExistAsync(List<int> medicamentIds)
    {
        var existingIds = await context.Medicaments
            .Where(m => medicamentIds.Contains(m.IdMedicament))
            .Select(m => m.IdMedicament)
            .ToListAsync();

        var missing = medicamentIds.Except(existingIds).ToList();
        if (missing.Any())
            throw new NotFoundException($"Nie znaleziono leków o ID: {string.Join(", ", missing)}");
    }

    private async Task<int> GetOrCreatePatientAsync(PatientDto patientDto)
    {
        if (patientDto.IdPatient.HasValue)
        {
            var exists = await context.Patients.AnyAsync(p => p.IdPatient == patientDto.IdPatient);
            if (exists)
                return patientDto.IdPatient.Value;
        }

        var newPatient = new Patient
        {
            FirstName = patientDto.FirstName,
            LastName = patientDto.LastName,
            Birthdate = DateOnly.FromDateTime(patientDto.Birthdate)
        };
        var entry = await context.Patients.AddAsync(newPatient);
        await context.SaveChangesAsync();
        return entry.Entity.IdPatient;
    }

    private Prescription CreatePrescriptionEntity(PostPrescriptionDto dto, int patientId, int doctorId)
    {
        return new Prescription
        {
            IdPatient = patientId,
            IdDoctor = doctorId,
            Date = DateOnly.FromDateTime(dto.Date.Value),
            DueDate = DateOnly.FromDateTime(dto.DueDate.Value),
            PrescriptionMedicaments = dto.Medicaments.Select(m => new PrescriptionMedicament
            {
                IdMedicament = m.IdMedicament,
                Dose = m.Dose,
                Details = m.Details
            }).ToList()
        };
    }
}