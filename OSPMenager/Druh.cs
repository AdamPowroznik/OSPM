using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPMenager
{
    public enum StatusDruha
    {
        Zwykły1,
        Zwykły2,
        Młody,
        Zarząd,
        CzłonekHonorowy,
    }
    public class Druh
    {

        private string imie;
        private string nazwisko;
        private DateTime dataUrodzin;
        private StatusDruha status;
        private DateTime dataTegoroczna;
        private int ileAkcji = 0;


        public string Imie {
            get { return imie; }
            set { imie = value; }
        }

        public string Nazwisko
        {
            get { return nazwisko; }
            set { nazwisko = value; }
        }

        public DateTime DataUrodzin
        {
            get { return dataUrodzin; }
            set { dataUrodzin = value; }
        }

        public StatusDruha Status
        {
            get { return status; }
            set { status = value; }
        }

        public int IleAkcji
        {
            get { return ileAkcji; }
            set { ileAkcji = value; }
        }


        public override string ToString()
        {
            return imie + " " + nazwisko;
        }


        public String ToString3()
        {
            int wiek;
            if (dataUrodzin.DayOfYear < DateTime.Today.DayOfYear)
            {
                wiek = DateTime.Today.Year + 1 - dataUrodzin.Year;
                dataTegoroczna = new DateTime(DateTime.Today.Year + 1, dataUrodzin.Month, dataUrodzin.Day);
            }
            else
            {
                wiek = DateTime.Today.Year - dataUrodzin.Year;
                dataTegoroczna = new DateTime(DateTime.Today.Year, dataUrodzin.Month, dataUrodzin.Day);
            }
            return imie+" "+nazwisko + "   " + dataTegoroczna.ToString("dddd, d.MM") + " skończy lat: "+wiek;
        }

        public String ToString2()
        {
            if (dataUrodzin.DayOfYear < DateTime.Today.DayOfYear)
            {
                int wiek = DateTime.Today.Year+1 - dataUrodzin.Year;
                dataTegoroczna = new DateTime(DateTime.Today.Year+1, dataUrodzin.Month, dataUrodzin.Day);
            }
            else
            {
                int wiek = (DateTime.Today.Year - dataUrodzin.Year);
                dataTegoroczna = new DateTime(DateTime.Today.Year, dataUrodzin.Month, dataUrodzin.Day);
            }
            return imie + " " + nazwisko + " w " + dataTegoroczna.ToString("dddd, d MMMM");
        }
    }
}
