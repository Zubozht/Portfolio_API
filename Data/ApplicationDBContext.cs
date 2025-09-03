using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> dbContextOptions)
        : base(dbContextOptions)
        {
            
        }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PhotoTag> PhotoTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Photo>()
                .Property<DateTime>("createdon")
                .HasColumnType("TIMESTAMP(6)")
                .HasDefaultValueSql("CURRENT_TIMESTAMP(6)")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<PhotoTag>()
                .HasKey(pt => new {pt.Photoid, pt.Tagid});

            modelBuilder.Entity<PhotoTag>()
                .HasOne(pt => pt.Photo)
                .WithMany(p => p.tags)
                .HasForeignKey(pt => pt.Photoid)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PhotoTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.photos)
                .HasForeignKey(pt => pt.Tagid)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppUser>().ToTable("aspnetusers");
            modelBuilder.Entity<IdentityRole>().ToTable("aspnetroles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("aspnetuserroles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("aspnetuserclaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("aspnetuserlogins");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("aspnetusertokens");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("aspnetroleclaims");

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);
                
        }
    }
}