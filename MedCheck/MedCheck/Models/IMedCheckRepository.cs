using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedCheck.Models
{
    public interface IMedCheckRepository
    {
        IQueryable<Hospital> Hospitals { get; }
        IQueryable<MedWorker> MedWorkers { get; }
        IQueryable<Speciality> Specialities { get; }
        IQueryable<User> Users { get; }
        IQueryable<Stats> Stats { get; }
    }
}
