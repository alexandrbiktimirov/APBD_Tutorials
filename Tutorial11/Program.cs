using Microsoft.EntityFrameworkCore;
using Tutorial11.Data;
using Tutorial11.Repositories;
using Tutorial11.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<DatabaseContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IMedicamentRepository, MedicamentRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();