using MedCheck.DAL;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedCheck.Models
{
    public class GetUserRecipes
    {
        private readonly MedCheckContext context;

        public GetUserRecipes(MedCheckContext db)
        {
            context = db;
        }

        public List<object> GetRecipe(string ID)
        {
            var list = new List<object>();

            list.Add(new[] { "DoctorNameFamilyName", "DoctorEmail", "Email", "Date", "PrescriptionItself" });


            //var testMed = context.Users.Where(x => x.Id == "c3b8b9b8-5521-4c70-a8b7-719f6840bc21").FirstOrDefault();

            //var testPat = context.Users.Where(x => x.Id == "ad7042bd-2b9e-4b0c-ac8d-af3a38278f0c").FirstOrDefault();

            var prescriptions = context.Prescriptions
                .Where(x => x.Patient.Id == ID).ToList();

            for (int i = 0; i < prescriptions.Count; i++)
            {
                list.Add(new object[] { prescriptions[i].MedWorker.Name + " " + prescriptions[i].MedWorker.FamilyName, prescriptions[i].MedWorker.Email, prescriptions[i].Patient.Email, prescriptions[i].Date, prescriptions[i].PrescriptionText });
            }

            //for (int i = 0; i < prescriptions.Count; i++)
            //{
            //    list.Add(new object[] { testMed.Name + " " + testMed.FamilyName, testMed.Email, testPat.Email, prescriptions[i].Date, prescriptions[i].PrescriptionText });
            //}

            if (prescriptions.Count == 0)
            {
                list.Add(new object[] { "No data", "No data", "No data", "No data", "No data" });
            }


            return list;
        }
    }
}
