using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OSPMenager
{
    /// <summary>
    /// Interaction logic for Wniosek1.xaml
    /// </summary>
    public partial class Wniosek1 : Window
    {
        List<Druh> druhowie = (List<Druh>)ReadWriteXml.readdata("", "");
        List<Unit> units = new List<Unit>();
      
        EquivalentApplication application = new EquivalentApplication()
        { Units = new List<Unit>()
        {
            //new Unit()
            //    {
            //        //Rescuer = (List<Druh>)ReadWriteXml.readdata("", ""),
            //        StartTime = new DateTime(2019, 02, 04,12,30,00),
            //        EndTime = new DateTime(2019, 02, 04,14,00,00),
            //    }
            },
            OccurrenceDateFromDay = DateTime.Today,
            RecordNumber = 998,
            Name = "Pożar trawy, ul. Bochotnica 10",
        };

        public Wniosek1()
        {
            int counter =0;
            foreach (var druh in druhowie)
            {
                if (counter < 10)
                {
                   // units.Add(new Unit() { Rescuer = druh, StartTime = new DateTime(2019, 02, 04, 12, 30, 00), EndTime = new DateTime(2019, 02, 04, 14, 00, 00), ID=counter+1});
                    counter++;
                }
            }
            application.Units = units;
            InitializeComponent();
            spContext.DataContext = application;
            dgRescuers.ItemsSource = application.Units;
           
        }

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "HH:mm";
            if (e.PropertyName == null)
                e.Column.Header = "Podpis";
        }

        
    }
}
