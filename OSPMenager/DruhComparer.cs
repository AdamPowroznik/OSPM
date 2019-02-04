using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPMenager
{
    public enum SortCriteria
    {
        TypeThenDate,
        DateThenType,
    }
    class DruhComparer : IComparer<Druh>
    {
        public SortCriteria SortBy = SortCriteria.DateThenType;

        public int Compare(Druh x, Druh y)
        {
            if (SortBy == SortCriteria.DateThenType)
            {
                if (x.DataUrodzin.DayOfYear > y.DataUrodzin.DayOfYear)
                    return 1;
                else if (x.DataUrodzin.DayOfYear < y.DataUrodzin.DayOfYear)
                    return -1;
                if (x.Status > y.Status)
                    return 1;
                else if (x.Status < y.Status)
                    return -1;
                else
                    return 0;
            }
            else
            {
                if (x.Status > y.Status)
                    return 1;
                else if (x.Status < x.Status)
                    return -1;
                else
                    if (x.DataUrodzin.DayOfYear > y.DataUrodzin.DayOfYear)
                    return 1;
                else if (x.DataUrodzin.DayOfYear < y.DataUrodzin.DayOfYear)
                    return -1;
                else
                    return 0;
            }
        }
    }
}
