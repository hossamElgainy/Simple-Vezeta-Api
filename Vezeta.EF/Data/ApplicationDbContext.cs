

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VezetaCore.Models;

namespace Vezeta.EF.Data
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Doctor> Doctor { get; set; }
        public DbSet<AppointMents> appointMents { get; set; }
        public DbSet<AppointMentTimes> appointMentTimes { get; set; }
        public DbSet<Discount> discount { get; set; }
        public DbSet<Booking> bookings { get; set; }
        public DbSet<DoctorPrice> DoctorPrice { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .Property(u => u.DateOfBirth)
                .HasColumnType("date"); // This assumes your database supports a "date" type.

            builder.Entity<ApplicationUser>()
                .Property(b => b.FullName)
                .HasComputedColumnSql("[FirstName]+','+ [LastName]");

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Admin",
                    NormalizedName = "admin" ,
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                },
                new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Doctor",
                    NormalizedName = "doctor",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                },
                new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Patient",
                    NormalizedName = "patient",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                });



            /*builder.Entity<Booking>()
                .HasOne(b => b.Appointment)
                .WithMany()
                .HasForeignKey(b => b.AppointmentsId)
                .OnDelete(DeleteBehavior.Restrict);*/

            /*builder.Entity<Booking>()
                .HasOne(b => b.Time)
                .WithMany()
                .HasForeignKey(b => b.AppointmentTimesId)
                .OnDelete(DeleteBehavior.Restrict);*/

            /* builder.Entity<ApplicationUser>().HasData(
                 new ApplicationUser()
                 {
                     Id = Guid.NewGuid().ToString(),
                     UserName = "Hossam@gmail.com",
                     NormalizedUserName = "HOSSAM@GMAIL.COM",
                     Email = "Hossam@gmail.com",
                     NormalizedEmail = "HOSSAM@GMAIL.COM",
                     EmailConfirmed = true,
                     PasswordHash = "",
                     SecurityStamp = "",
                     ConcurrencyStamp = Guid.NewGuid().ToString(),
                     PhoneNumber = "01127929624",
                     PhoneNumberConfirmed = true,
                     TwoFactorEnabled = false,
                     LockoutEnd = null,
                     LockoutEnabled = true,
                     AccessFailedCount = 0,
                     FirstName = "Hossam",
                     LastName = "Elganiny",
                     Gender = Gender.Male,
                     Image = "",
                     DateOfBirth = DateTime.Now,
                 });*/
        }
    }
}