using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPMenager
{
    public class Unit
    {
        public Unit(Druh rescuer, DateTime startTime, DateTime endTime)
        {
            Rescuer = rescuer;
            StartTime = startTime;
            EndTime = endTime;
        }
        public Druh Rescuer { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan TotalTime { get { return EndTime - StartTime; } set { } }
        public int ID { get; set; }


        public override string ToString()
        {
            return Rescuer.Imie + " " + Rescuer.Nazwisko + "  Od: " + StartTime.ToShortTimeString() + " Do: " + EndTime.ToShortTimeString();
        }
    }
}