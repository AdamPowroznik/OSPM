using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace OSPMenager
{
    public class EquivalentApplication
    {
        TimeSpan totalTime;
        public String DateAndPlace { get { return "Nałęczów, " + OccurrenceDateFromDay.ToShortDateString(); } set { } }
        public DateTime OccurrenceDateFromDay { get; set; }
        public List<Unit> Units { get; set; }
        public String RecordNumber { get; set; }
        public String Name { get; set; }
        public TimeSpan TotalTime
        {
            get
            {
                totalTime = new TimeSpan(0);
                foreach (var init in Units)
                {
                    totalTime += init.TotalTime;
                }
                return totalTime;
            } set { } }

        public String OccurenceDate { get { return OccurrenceDateFromDay.ToShortDateString(); } set { } }

        public EquivalentApplication(DateTime occurenceDateFromDay, List<Unit> units, String recordNumber, String name)
        {
            OccurrenceDateFromDay = occurenceDateFromDay;
            Units = units;
            RecordNumber = recordNumber;
            Name = name;
        }

        public EquivalentApplication() { }
        
    }
}
