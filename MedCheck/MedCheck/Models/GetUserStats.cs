using MedCheck.DAL;

using System;
using System.Collections.Generic;
using System.Globalization;
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
        
        public List<object> GetGraphStats(string id, int showEntries)
        {
            var list = new List<object>();

            list.Add(new[] { "Date", $"{Type}" });

            List<Stats> data = context.Stats
                .Where(stats => stats.UserId == id)
                .ToList();

            var uniqueDataFilter = from entry in data
                             select new { DateNoTime = new DateTime(entry.Date.Year, entry.Date.Month, entry.Date.Day) };

            var uniqueData = uniqueDataFilter.Distinct().ToList();

            var o = new { Avg = 1.0 };

            var avgData = new[] { o }.ToList();

            avgData.RemoveAt(avgData.Count - 1);

            if (Type == "Temperature")
            {
                var avgDataFilter = from entry in data
                                    let compare = new DateTime(entry.Date.Year, entry.Date.Month, entry.Date.Day)
                              group entry by compare into g
                              select new { Avg = g.Average(n => n.Temperature) };

                avgData = avgDataFilter.ToList();
            }
            else if (Type == "Pressure")
            {
                var avgDataFilter = from entry in data
                                    let compare = new DateTime(entry.Date.Year, entry.Date.Month, entry.Date.Day)
                                    group entry by compare into g
                                    select new { Avg = g.Average(n => n.Pressure) };

                avgData = avgDataFilter.ToList();
            }
            else if (Type == "Oxygen")
            {
                var avgDataFilter = from entry in data
                                    let compare = new DateTime(entry.Date.Year, entry.Date.Month, entry.Date.Day)
                                    group entry by compare into g
                                    select new { Avg = g.Average(n => n.OxygenLevel) };

                avgData = avgDataFilter.ToList();
            }
            else if (Type == "Pulse")
            {
                var avgDataFilter = from entry in data
                                    let compare = new DateTime(entry.Date.Year, entry.Date.Month, entry.Date.Day)
                                    group entry by compare into g
                                    select new { Avg = g.Average(n => n.Pulse) };

                avgData = avgDataFilter.ToList();
            }

            if (uniqueData.Count != 0 && uniqueData.Count - 10 * (showEntries + 1) >= 0)
            {
                for (int i = uniqueData.Count - 10 * (showEntries + 1) - 1; i < uniqueData.Count - 10 * (showEntries); i++)
                {
                    DateTime date = uniqueData[i].DateNoTime;

                    date = new DateTime(date.Year, date.Month, date.Day);

                    string returnDate = $"{date.Day}/{date.Month}/{date.Year}";

                    list.Add(new object[] { returnDate, avgData[i].Avg });
                }
            }
            else if (uniqueData.Count != 0 && uniqueData.Count - 10 * (showEntries + 1) <= 0)
            {
                for (int i = uniqueData.Count - 10 * (showEntries + 1) - 1 - (uniqueData.Count - 10 * (showEntries + 1) - 1); i < uniqueData.Count - 10 * (showEntries); i++)
                {
                    DateTime date = uniqueData[i].DateNoTime;

                    date = new DateTime(date.Year, date.Month, date.Day);

                    string returnDate = $"{date.Day}/{date.Month}/{date.Year}";

                    list.Add(new object[] { returnDate, avgData[i].Avg });
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

        public List<object> GetMiniGraphStats(string id, int showEntries, string dateTime)
        {
            var list = new List<object>();

            var lol = dateTime.Length;

            list.Add(new[] { "Date", $"{Type}" });

            DateTime oDate = DateTime.ParseExact(dateTime, "d/M/yyyy", CultureInfo.CurrentCulture);

            List<Stats> data = context.Stats
                .Where(stats => stats.UserId == id && stats.Date.Year == oDate.Year && stats.Date.Month == oDate.Month && stats.Date.Day == oDate.Day)
                .ToList();

            if (data.Count != 0 && data.Count - 10 * (showEntries + 1) >= 0)
            {
                for (int i = data.Count - 10 * (showEntries + 1); i < data.Count - 10 * (showEntries); i++)
                {
                    DateTime date = data[i].Date;

                    string returnDate = "";

                    if (date.Minute < 10)
                    {
                        returnDate = $"{date.Hour}:0{date.Minute}";
                    }
                    else
                    {
                        returnDate = $"{date.Hour}:{date.Minute}";
                    }

                    if (Type == "Temperature")
                    {
                        list.Add(new object[] { returnDate, data[i].Temperature });
                    }
                    else if (Type == "Pressure")
                    {
                        list.Add(new object[] { returnDate, data[i].Pressure });
                    }
                    else if (Type == "Oxygen")
                    {
                        list.Add(new object[] { returnDate, data[i].OxygenLevel });
                    }
                    else if (Type == "Pulse")
                    {
                        list.Add(new object[] { returnDate, data[i].Pulse });
                    }

                }
            }
            else if (data.Count != 0 && data.Count - 10 * (showEntries + 1) <= 0)
            {
                for (int i = data.Count - 10 * (showEntries + 1) - 1 - (data.Count - 10 * (showEntries + 1) - 1); i < data.Count - 10 * (showEntries); i++)
                {
                    DateTime date = data[i].Date;

                    string returnDate = "";

                    if (date.Minute < 10)
                    {
                        returnDate = $"{date.Hour}:0{date.Minute}";
                    }
                    else
                    {
                        returnDate = $"{date.Hour}:{date.Minute}";
                    }
                    

                    if (Type == "Temperature")
                    {
                        list.Add(new object[] { returnDate, data[i].Temperature });
                    }
                    else if (Type == "Pressure")
                    {
                        list.Add(new object[] { returnDate, data[i].Pressure });
                    }
                    else if (Type == "Oxygen")
                    {
                        list.Add(new object[] { returnDate, data[i].OxygenLevel });
                    }
                    else if (Type == "Pulse")
                    {
                        list.Add(new object[] { returnDate, data[i].Pulse });
                    }
                }
            }

            return list;
        }
    }
}
