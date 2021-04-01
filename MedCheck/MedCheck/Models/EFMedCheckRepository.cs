using MedCheck.DAL;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedCheck.Models
{
    public class EFMedCheckRepository : IMedCheckRepository
    {
        private readonly MedCheckContext context;

        public EFMedCheckRepository(MedCheckContext ctx)
        {
            context = ctx;
        }

        public IQueryable<User> Users => context.Users;

        public IQueryable<Hospital> Hospitals => context.Hospitals;

        public IQueryable<MedWorker> MedWorkers => context.MedWorkers;

        public IQueryable<Speciality> Specialities => context.Specialities;

        public IQueryable<Stats> Stats => context.Stats;
    }
}
