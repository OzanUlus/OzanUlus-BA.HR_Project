﻿using BA.HR_Project.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BA.HR_Project.Persistance.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder
                .HasOne(x => x.Adress)
                .WithOne(x => x.AppUser)
                .HasForeignKey<AppUser>(x => x.AdressId);

            builder
                .HasOne(x => x.Company)
                .WithMany(x => x.AppUsers)
                .HasForeignKey(x => x.CompanyId);
            builder
                .HasOne(x => x.Department)
                .WithMany(x => x.AppUsers)
                .HasForeignKey(x => x.DepartmentId);
            builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Surname).HasMaxLength(50).IsRequired();
            builder.Property(x => x.BirthPlace).HasMaxLength(50);
            builder.Property(x => x.Email).HasMaxLength(50).IsRequired();
            builder.HasIndex(x => x.Email).IsUnique();

            var hasher = new PasswordHasher<AppUser>();

            var adminRoleID = Guid.NewGuid().ToString();


            builder.HasData(new
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "admin@bilgeadam.com",
                BirthDate = DateTime.Now,
                NormalizedUserName = "ADMIN@BILGEADAM.COM",
                Email = "admin@bilgeadam.com",
                NormalizedEmail = "ADMIN@BILGEADAM.COM",
                EmailConfirmed = true,
                PasswordHash = "Admin",
                SecurityStamp = string.Empty,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Name = "Admin",
                Surname = "Bilgeadam",
                AccessFailedCount = 3,
                LockoutEnabled = false,
                IsTurkishCitizen = true,
                PhoneNumber ="0",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled=false,
                

                AdressId = "SeedAdress1",
                DepartmentId = "SeedDepartment1",
                CompanyId = "SeedCompany1"
            });
        }
    }
}
