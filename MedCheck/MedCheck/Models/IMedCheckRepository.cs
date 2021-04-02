using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedCheck.Models
{
    public interface IMedCheckRepository
    {
        IQueryable<Hospital> Hospitals { get; }
        IQueryable<Speciality> Specialities { get; }
        IQueryable<MainUser> MainUsers { get; }
        IQueryable<Stats> Stats { get; }
    }
}
