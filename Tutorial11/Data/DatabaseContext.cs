using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Tutorial11.Models;

namespace Tutorial11.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    // protected DatbaseContext()
    // {
    //     
    // }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>(p =>
        {
            p.ToTable("Patients");

            p.HasKey(e => e.IdPatient);
            p.Property(e => e.FirstName).HasMaxLength(50);
            p.Property(e => e.LastName).HasMaxLength(50);
            p.Property(e => e.BirthDate).IsRequired();
        });

        modelBuilder.Entity<Patient>().HasData(new List<Patient>()
        {
            new Patient()
                { IdPatient = 1, FirstName = "John", LastName = "Doe", BirthDate = DateTime.Now.AddYears(-30) },
            new Patient()
                { IdPatient = 2, FirstName = "Jane", LastName = "Doe", BirthDate = DateTime.Now.AddYears(-25) },
        });

        modelBuilder.Entity<Doctor>().HasData(new List<Doctor>()
        {
            new Doctor() { IdDoctor = 1, FirstName = "Joseph", LastName = "Does", Email = "joseph.doe@gmail.com" },
            new Doctor() { IdDoctor = 2, FirstName = "Jessica", LastName = "Doe", Email = "jess.doe@gmail.com" },
        });

        modelBuilder.Entity<Medicament>().HasData(new List<Medicament>()
        {
            new Medicament() {IdMedicament = 1, Name = "Aspirin", Description = "Aspirin is in a group of medications called salicylates. It works by stopping the production of certain natural substances that cause fever, pain, swelling, and blood clots. Aspirin is also available in combination with other medications such as antacids, pain relievers, and cough and cold medications.", Type="Nonsteroidal anti-inflammatory drug" },
            new Medicament() {IdMedicament = 2, Name = "Paracetamol", Description = "Paracetamol is a medicine used to treat mild to moderate pain. Paracetamol can also be used to treat fever (high temperature). It's dangerous to take more than the recommended dose of paracetamol. Paracetamol overdose can damage your liver and cause death.", Type = "Non-opioid analgesic and antipyretic agent"}
        });

        modelBuilder.Entity<Prescription>().HasData(new List<Prescription>()
        {
            new Prescription()
            {
                IdPrescription = 1, Date = new DateTime(2025, 5, 19), DueDate = new DateTime(2025, 6, 2), IdPatient = 1,
                IdDoctor = 1
            },
            new Prescription()
            {
                IdPrescription = 2, Date = new DateTime(2025, 5, 18), DueDate = new DateTime(2025, 6, 1), IdPatient = 2,
                IdDoctor = 2,
            },
        });

        modelBuilder.Entity<PrescriptionMedicament>().HasData(new List<PrescriptionMedicament>()
        {
            new PrescriptionMedicament() { }
        });
    }
}