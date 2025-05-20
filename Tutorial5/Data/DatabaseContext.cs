using Microsoft.EntityFrameworkCore;
using Tutorial5.Models;

namespace Tutorial5.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    protected DatabaseContext() { }

    public DatabaseContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PrescriptionMedicament>()
            .HasKey(pm => new { pm.IdPrescription, pm.IdMedicament });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
        });

        modelBuilder.Entity<Medicament>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(100);
        });

        modelBuilder.Entity<Doctor>().HasData(new List<Doctor>
        {
            new Doctor(){IdDoctor = 1, Email = "john.doe@gmail.com", FirstName = "John", LastName = "Doe"},
            new Doctor(){IdDoctor = 2, Email = "another.doe@gmail.com", FirstName = "Another", LastName = "Doe"},
            new Doctor(){IdDoctor = 3, Email = "everyone@gmail.com", FirstName = "Everyone", LastName = "Doe"},
        });

        modelBuilder.Entity<Patient>().HasData(new List<Patient>
        {
            new Patient(){IdPatient = 1, FirstName = "Peter", LastName = "Doe", Birthdate = new DateOnly(1981, 9, 29) },
            new Patient(){IdPatient = 2, FirstName = "Hanna", LastName = "Doe", Birthdate = new DateOnly(1982, 9, 29) },
            new Patient(){IdPatient = 3, FirstName = "Anna", LastName = "Doe", Birthdate = new DateOnly(1983, 9, 29) },
        });

        modelBuilder.Entity<Prescription>().HasData(new List<Prescription>
        {
            new Prescription()
            {
                IdPrescription = 1, Date = new DateOnly(2025, 5, 5), DueDate = new DateOnly(2025, 6, 6), IdDoctor = 1,
                IdPatient = 2
            },
            new Prescription()
            {
                IdPrescription = 2, Date = new DateOnly(2024, 6, 6), DueDate = new DateOnly(2025, 1, 1), IdDoctor = 2,
                IdPatient = 3
            }
        });

        modelBuilder.Entity<Medicament>().HasData(new List<Medicament>()
        {
            new Medicament()
            {
                IdMedicament = 1, Name = "Paracetamol", Type = "Pain Killer", Description = ""
            },
            new Medicament()
            {
                IdMedicament = 2, Name = "Ibuprofen", Type = "Pain Killer", Description = ""
            }
        });
    }
}