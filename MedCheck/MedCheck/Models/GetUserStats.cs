using MedCheck.DAL;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedCheck.Models
{
    public class GetUserStats
    {
        private readonly MedCheckContext context;

        public GetUserStats(MedCheckContext db, string graphType)
        {
            context = db;

            Type = graphType;
        }

        public string Type { get; set; }
        
        public List<object> GetGraphStats(string id)
        {
            var list = new List<object>();

            list.Add(new[] { "Date", $"{Type}" });

            List<Stats> data = context.Stats
                .Where(stats => stats.UserId == id)
                .ToList();

            if (data.Count != 0)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    DateTime date = data[i].Date;

                    date = new DateTime(date.Year, date.Month, date.Day);

                    string returnDate = $"{date.Day}/{date.Month}/{date.Year}";

                    if(Type == "Temperature")
                    {
                        list.Add(new object[] { returnDate, data[i].Temperature });
                    }
                    else if(Type == "Pressure")
                    {
                        list.Add(new object[] { returnDate, data[i].Pressure });
                    }
                    else if(Type == "Oxygen")
                    {
                        list.Add(new object[] { returnDate, data[i].OxygenLevel });
                    }
                    else if(Type == "Pulse")
                    {
                        list.Add(new object[] { returnDate, data[i].Pulse });
                    }
                }
            }
            else
            {
                DateTime date = DateTime.UtcNow;

                date = new DateTime(date.Year, date.Month, date.Day);

                string returnDate = $"{date.Day}/{date.Month}/{date.Year}";
                if (Type == "Temperature")
                {
                    list.Add(new object[] { returnDate, 36.6 });
                }
                else if (Type == "Pressure")
                {
                    list.Add(new object[] { returnDate, 100});
                }
                else if (Type == "Oxygen")
                {
                    list.Add(new object[] { returnDate, 100});
                }
                else if (Type == "Pulse")
                {
                    list.Add(new object[] { returnDate, 100 });
                }
            }

            return list;
        }


    }
}
