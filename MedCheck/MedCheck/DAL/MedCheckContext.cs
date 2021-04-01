﻿using MedCheck.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedCheck.DAL
{
    public class MedCheckContext : IdentityDbContext<User>
    {
        public MedCheckContext(DbContextOptions<MedCheckContext> options)
            : base(options) { }

        public DbSet<MedWorker> MedWorkers { get; set; }

        public DbSet<Hospital> Hospitals { get; set; }

        public DbSet<Stats> Stats { get; set; }

        public DbSet<Speciality> Specialities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Stats>()
                .HasOne(stats => stats.User)
                .WithMany(user => user.Stats);
        }
    }
}