using BlogApp.DataApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.DataApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<AboutMe> AboutMe { get; set; }
        public DbSet<PersonalInfo> PersonalInfo { get; set; }
        public DbSet<Experiences> Experiences { get; set; }
        public DbSet<Educations> Educations { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<ContactMessages> ContactMessages { get; set; }
    }
}
