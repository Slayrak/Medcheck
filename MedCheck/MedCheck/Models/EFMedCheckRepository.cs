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

        public IQueryable<MainUser> MainUsers => context.Users;

        public IQueryable<Hospital> Hospitals => context.Hospitals;

        public IQueryable<Speciality> Specialities => context.Specialities;

        public IQueryable<Stats> Stats => context.Stats;

        public IQueryable<Prescription> Prescriptions => context.Prescriptions;

    }
}
