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
        EquivalentApplication application = new EquivalentApplication();

        public Wniosek1(EquivalentApplication Application)
        {
            
            InitializeComponent();
            application = Application;
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
