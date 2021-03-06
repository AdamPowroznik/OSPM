﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Serialization;



namespace OSPMenager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Druh> druhowie = new List<Druh>();
        DispatcherTimer timer = new DispatcherTimer();
        DruhComparer comparer = new DruhComparer();
        DruhComparer comparer2 = new DruhComparer();
        bool edytowanie;
        Druh edytowany = new Druh();
        Style s;
        bool StateClosed = true;

        public MainWindow()
        {
            //Zegareczek
            timer.Tick += new EventHandler(timer_Click);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
            s = (Style)this.Resources["CalendarDayButtonStyle"];
            InitializeComponent();
            PomalujDni(druhowie);
            Clock();
            dpApplication.SelectedDate = DateTime.Today;

            //dg.ItemsSource = druhowie;

        }

        List<Druh> commingSoonList = new List<Druh>();
        private void UpdateControls()
        {
            lvCommingSoon.Items.Clear();
            comparer.SortBy = SortCriteria.DateThenType;
            druhowie.Sort(comparer);
            List<Druh> koniecKolejki = new List<Druh>();
            commingSoonList = new List<Druh>();
            foreach (var item in druhowie)
            {
                if (item.DataUrodzin.DayOfYear >= DateTime.Today.DayOfYear)
                {
                    lvCommingSoon.Items.Add(item.ToString3());
                    commingSoonList.Add(item);
                }
                else
                    koniecKolejki.Add(item);
            }
            koniecKolejki.Sort(comparer);
            foreach (var item in koniecKolejki)
            {
                lvCommingSoon.Items.Add(item.ToString3());
                commingSoonList.Add(item);
            }
            foreach (var item in druhowie)
            {
                if (item.DataUrodzin.DayOfYear > DateTime.Today.DayOfYear)
                {
                    tbSoonBd.Text = "Najbliższe urodziny obchodzi druh " + item.ToString2();
                    break;
                }

            }

            tbGreeting.Text = "Dziś jest " + DateTime.Now.ToString("dddd, dd MMMM yyyy");
        }

        private void timer_Click(object sender, EventArgs e)
        {
            Clock();
            UpdateControls();
        }

        private void Clock()
        {
            DateTime teraz = DateTime.Now;
            if (teraz.Minute < 10)
                tbClock.Text = teraz.Hour + ":0" + teraz.Minute;
            else
                tbClock.Text = teraz.Hour + ":" + teraz.Minute;

        }

        private void calendar_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            //KONFIGURACJA WYŚWIETLANIA DRUGIEGO KALENDARZA
            if (calendar.DisplayDate.Month < 12)
                calendar1.DisplayDate = new DateTime(calendar.DisplayDate.Year, calendar.DisplayDate.Month + 1, 1);
            else
                calendar1.DisplayDate = new DateTime(calendar.DisplayDate.Year + 1, 1, 1);
        }
        //konfiguracja kalendarzy
        private void calendar1_Loaded(object sender, RoutedEventArgs e)
        {
            calendar.BlackoutDates.Add(new CalendarDateRange(new DateTime(1990, 1, 1), DateTime.Now.AddDays(-1)));
            calendar1.BlackoutDates.Add(new CalendarDateRange(new DateTime(1990, 1, 1), DateTime.Now.AddDays(-1)));
            calendar.SelectedDate = DateTime.Today;
            calendar1.DisplayDateStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month + 1, 1);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            save();
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            dpDataNowego.SelectedDate = calendar.SelectedDate;
            btCancelNewEvent.Visibility = Visibility.Visible;
            gAddMember.Visibility = Visibility.Visible;
        }

        private void btAcceptNewEvent_Click(object sender, RoutedEventArgs e)
        {
            if (edytowanie)
                druhowie.Remove(edytowany);
            Druh nowy = new Druh();
            nowy.Imie = tbImieNowego.Text;
            nowy.Nazwisko = tbNazwiskoNowego.Text;
            nowy.DataUrodzin = dpDataNowego.SelectedDate.Value;
            switch (cbStatusNowego.SelectedIndex)
            {
                case 0:
                    nowy.Status = StatusDruha.Zwykły1;
                    break;
                case 1:
                    nowy.Status = StatusDruha.Zwykły2;
                    break;
                case 2:
                    nowy.Status = StatusDruha.Młody;
                    break;
                case 3:
                    nowy.Status = StatusDruha.Zarząd;
                    break;
                case 4:
                    nowy.Status = StatusDruha.CzłonekHonorowy;
                    break;
            }
            druhowie.Add(nowy);
            save();
            tbImieNowego.Text = "";
            tbNazwiskoNowego.Text = "";
            dpDataNowego.SelectedDate = calendar.SelectedDate;
            cbStatusNowego.SelectedIndex = 0;
            gAddMember.Visibility = Visibility.Collapsed;
            UpdateControls();
        }

        private void btCancelNewEvent_Click(object sender, RoutedEventArgs e)
        {
            gAddMember.Visibility = Visibility.Collapsed;
            tbImieNowego.Text = "";
            tbNazwiskoNowego.Text = "";
            dpDataNowego.SelectedDate = calendar.SelectedDate;
            cbStatusNowego.SelectedIndex = 0;
            gAddMember.Visibility = Visibility.Collapsed;
        }

        private void save()
        {
            try
            {
                ReadWriteXml.savedata(druhowie, "supernazwa.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btEdit_Click(object sender, RoutedEventArgs e)
        {

        }


        private void Window_Initialized(object sender, EventArgs e)
        {
            druhowie = (List<Druh>)ReadWriteXml.readdata("", "");
            List<Druh> koniecKolejki = new List<Druh>();
            foreach (var item in druhowie)
            {
                if (item.DataUrodzin.DayOfYear >= DateTime.Today.DayOfYear)
                    lvCommingSoon.Items.Add(item.ToString3());
                else
                    koniecKolejki.Add(item);
            }
            koniecKolejki.Sort(comparer);
            foreach (var item in koniecKolejki)
            {
                lvCommingSoon.Items.Add(item.ToString3());
            }
        }

        private void btRemove_Click(object sender, RoutedEventArgs e)
        {
            druhowie.Remove(edytowany);
            commingSoonList.RemoveAt(index);
            UpdateControls();
        }

        private void lvCommingSoon_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //edytowanie = true;
            //edytowany = (Druh)lvCommingSoon.SelectedItem;
            //tbImieNowego.Text = edytowany.Imie;
            //tbNazwiskoNowego.Text = edytowany.Nazwisko;
            //dpDataNowego.SelectedDate = edytowany.DataUrodzin;
            //cbStatusNowego.SelectedIndex = (int)edytowany.Status;

            //btCancelNewEvent.Visibility = Visibility.Hidden;
            //gAddMember.Visibility = Visibility.Visible;
        }
        int index = 0;
        private void lvCommingSoon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lvCommingSoon.SelectedIndex >= 0)
            {
                index = lvCommingSoon.SelectedIndex;
                edytowany = commingSoonList[index];
            }
            
         //   if (lvCommingSoon.SelectedIndex >= 0)
          //      edytowany = commingSoonList.(lvCommingSoon.SelectedIndex);
                    //(Druh)lvCommingSoon.SelectedItem;
        }

        private void PomalujDni(List<Druh> wydarzenia)
        {
            Style s = (Style)this.Resources["CalendarDayButtonStyle"];

            foreach (Druh druh in druhowie)
            {
                DateTime dataTegoroczna;
                if (druh.DataUrodzin.DayOfYear < DateTime.Today.DayOfYear)
                {
                    dataTegoroczna = new DateTime(DateTime.Today.Year + 1, druh.DataUrodzin.Month, druh.DataUrodzin.Day);
                }
                else
                {
                    dataTegoroczna = new DateTime(DateTime.Today.Year, druh.DataUrodzin.Month, druh.DataUrodzin.Day);
                }

                if (druh.Status == StatusDruha.Zwykły1)
                {
                    DataTrigger dataTrigger = new DataTrigger() { Binding = new Binding("Date"), Value = dataTegoroczna };
                    dataTrigger.Setters.Add(new Setter(CalendarDayButton.BackgroundProperty, Brushes.Red));
                    s.Triggers.Add(dataTrigger);
                }

                else if (druh.Status == StatusDruha.Zwykły2)
                {
                    DataTrigger dataTrigger = new DataTrigger() { Binding = new Binding("Date"), Value = dataTegoroczna };
                    dataTrigger.Setters.Add(new Setter(CalendarDayButton.BackgroundProperty, Brushes.LawnGreen));
                    s.Triggers.Add(dataTrigger);
                }

                else if (druh.Status == StatusDruha.Młody)
                {
                    DataTrigger dataTrigger = new DataTrigger() { Binding = new Binding("Date"), Value = dataTegoroczna };
                    dataTrigger.Setters.Add(new Setter(CalendarDayButton.BackgroundProperty, Brushes.Gold));
                    s.Triggers.Add(dataTrigger);
                }

                else if (druh.Status == StatusDruha.Zarząd)
                {
                    DataTrigger dataTrigger = new DataTrigger() { Binding = new Binding("Date"), Value = dataTegoroczna };
                    dataTrigger.Setters.Add(new Setter(CalendarDayButton.BackgroundProperty, Brushes.HotPink));
                    s.Triggers.Add(dataTrigger);
                }

                else if (druh.Status == StatusDruha.CzłonekHonorowy)
                {
                    DataTrigger dataTrigger = new DataTrigger() { Binding = new Binding("Date"), Value = dataTegoroczna };
                    dataTrigger.Setters.Add(new Setter(CalendarDayButton.BackgroundProperty, Brushes.LightSkyBlue));
                    s.Triggers.Add(dataTrigger);
                }
            }
        }

        private void ButtonMenu_Click(object sender, RoutedEventArgs e)
        {
            if (StateClosed)
            {
                Storyboard sb = this.FindResource("OpenMenu") as Storyboard;
                sb.Begin();
                ButtonMenu2.Visibility = Visibility.Visible;
                ButtonMenu3.Visibility = Visibility.Visible;
                ButtonMenu4.Visibility = Visibility.Visible;
                tbWelcomeTitle.Visibility = Visibility.Visible;
            }
            else
            {

                tbWelcomeTitle.Visibility = Visibility.Hidden;
                ButtonMenu2.Visibility = Visibility.Hidden;
                ButtonMenu3.Visibility = Visibility.Hidden;
                ButtonMenu4.Visibility = Visibility.Hidden;
                Storyboard sb = this.FindResource("CloseMenu") as Storyboard;
                sb.Begin();
                
            }

            StateClosed = !StateClosed;
        }
        private void ButtonMenu2_Click(object sender, RoutedEventArgs e)
        {
            ButtonMenu_Click(this, null);
            OSPUrodziny.Visibility = Visibility.Visible;
            gStartGrid.Visibility = Visibility.Hidden;
            OSPEquivalent.Visibility = Visibility.Hidden;
            gINFO.Visibility = Visibility.Hidden;
        }

        private void btQuit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.Close();
            App.Current.Shutdown();
        }

        private void ButtonMenu3_Click(object sender, RoutedEventArgs e)
        {
            ButtonMenu_Click(this, null);
            gStartGrid.Visibility = Visibility.Hidden;
            OSPUrodziny.Visibility = Visibility.Hidden;
            OSPEquivalent.Visibility = Visibility.Visible;
            gINFO.Visibility = Visibility.Hidden;
        }

        private void btnEquivalentMenu2_Click(object sender, RoutedEventArgs e)
        {
            gMainInfo.Visibility = Visibility.Hidden;
            gAddedUnits.Visibility = Visibility.Hidden;
            gRemoveUnits.Visibility = Visibility.Hidden;
            if(dpEquivalentOccurrence.SelectedDate != null)
            {
                dpEndTime.SelectedDate = (DateTime)dpEquivalentOccurrence.SelectedDate;
                dpStartTime.SelectedDate = (DateTime)dpEquivalentOccurrence.SelectedDate;
            }
                
            spUnits.Items.Clear();
            comparer2.SortBy = SortCriteria.ActionsThenType;
            druhowie.Sort(comparer2);
            foreach (var druh in druhowie)
            {
                if (zastep.Contains(druh) == false)
                {
                    CheckBox chk = new CheckBox();
                    chk.Content = druh;
                    //chk.FontSize = 24;
                    ScaleTransform scale = new ScaleTransform(1.2, 1.2);
                    //chk.RenderTransformOrigin = new Point(0.5, 0.5);
                    chk.RenderTransform = scale;
                    chk.Padding = new Thickness(5, 0, 0, 0);
                    chk.Width = 350;
                    chk.VerticalAlignment = VerticalAlignment.Center;
                    spUnits.Items.Add(chk);
                }
            }
            gAddUnits.Visibility = Visibility.Visible;
        }


        List<Druh> zastep = new List<Druh>();
        List<Unit> units = new List<Unit>();
        private void btApproveUnit_Click(object sender, RoutedEventArgs e)
        {
            // int t = 0;
            // if (dpStartTime.SelectedDate != null)
            //     if(dpEndTime.SelectedDate != null)
            //         if(Int32.TryParse(tbStartHour.Text, out t))
            //             if(Int32.TryParse(tbStartMinute.Text, out t))
            //                 if(Int32.TryParse(tbEndHour.Text, out t))
            //                     if(Int32.TryParse(tbEndMinute.Text, out t))
            //                         if(Int32.Parse(tbStartHour.Text) < 24)
            //                             if(Int32.Parse(tbStartHour.Text) >= 0)
            //                                 if(Int32.Parse(tbStartMinute.Text) <60)
            //                                     if(Int32.Parse(tbStartMinute.Text) >= 0)
            //                                         if (Int32.Parse(tbEndHour.Text) < 24)
            //                                             if (Int32.Parse(tbEndHour.Text) >= 0)
            //                                                 if (Int32.Parse(tbEndMinute.Text) < 60)
            //                                                     if (Int32.Parse(tbEndMinute.Text) >= 0)
            //                                                     {
            //                                                         foreach (CheckBox item in spUnits.Items)
            //                                                         {
            //                                                             if (item.IsChecked == true)
            //                                                             {
            //                                                                 units.Add(new Unit((Druh)item.Content,
            //                                                                     new DateTime(dpStartTime.SelectedDate.Value.Year, dpStartTime.SelectedDate.Value.Month, dpStartTime.SelectedDate.Value.Day, Int32.Parse(tbStartHour.Text), Int32.Parse(tbStartMinute.Text), 0),
            //                                                                     new DateTime(dpEndTime.SelectedDate.Value.Year, dpEndTime.SelectedDate.Value.Month, dpEndTime.SelectedDate.Value.Day, Int32.Parse(tbEndHour.Text), Int32.Parse(tbEndMinute.Text), 0)));
            //                                                                 zastep.Add((Druh)item.Content);
            //                                                             }
            //                                                         }
            //                                                         gMainInfo.HorizontalAlignment = HorizontalAlignment.Left;
            //                                                         gAddedUnits.HorizontalAlignment = HorizontalAlignment.Right;
            //                                                         dgRescuers.ItemsSource = null;
            //                                                         dgRescuers.Items.Clear();
            //                                                         dgRescuers.ItemsSource = units;
            //                                                         gAddUnits.Visibility = Visibility.Hidden;
            //                                                         gAddedUnits.Visibility = Visibility.Visible;
            //                                                         gMainInfo.Visibility = Visibility.Visible;
            //                                                     }
            //else
            // {
            //    MessageBox.Show("Błędnie wypełnione daty lub godziny!", "Erroł", MessageBoxButton.OK, MessageBoxImage.Information);
            // }

            try
            {
                Unit test = new Unit(new Druh(),
                            new DateTime(dpStartTime.SelectedDate.Value.Year, dpStartTime.SelectedDate.Value.Month, dpStartTime.SelectedDate.Value.Day, Int32.Parse(tbStartHour.Text), Int32.Parse(tbStartMinute.Text), 0),
                            new DateTime(dpEndTime.SelectedDate.Value.Year, dpEndTime.SelectedDate.Value.Month, dpEndTime.SelectedDate.Value.Day, Int32.Parse(tbEndHour.Text), Int32.Parse(tbEndMinute.Text), 0));
                if(test.EndTime > test.StartTime)
                {
                    int counter = 0;
                    foreach (CheckBox item in spUnits.Items)
                    {
                        if (item.IsChecked == true)
                            counter++;
                    }
                    if (counter >= 3)
                    {
                        foreach (CheckBox item in spUnits.Items)
                        {
                            if (item.IsChecked == true)
                            {
                                units.Add(new Unit((Druh)item.Content,
                                    new DateTime(dpStartTime.SelectedDate.Value.Year, dpStartTime.SelectedDate.Value.Month, dpStartTime.SelectedDate.Value.Day, Int32.Parse(tbStartHour.Text), Int32.Parse(tbStartMinute.Text), 0),
                                    new DateTime(dpEndTime.SelectedDate.Value.Year, dpEndTime.SelectedDate.Value.Month, dpEndTime.SelectedDate.Value.Day, Int32.Parse(tbEndHour.Text), Int32.Parse(tbEndMinute.Text), 0)));
                                zastep.Add((Druh)item.Content);
                            }
                        }
                        gMainInfo.HorizontalAlignment = HorizontalAlignment.Left;
                        gAddedUnits.HorizontalAlignment = HorizontalAlignment.Right;
                        dgRescuers.ItemsSource = null;
                        dgRescuers.Items.Clear();
                        dgRescuers.ItemsSource = units;
                        gAddUnits.Visibility = Visibility.Hidden;
                        gAddedUnits.Visibility = Visibility.Visible;
                        gMainInfo.Visibility = Visibility.Visible;
                    }
                    else
                        MessageBox.Show("Zastęp musi składać się z minimum trzech członków.", "Coś poszło nie tak", MessageBoxButton.OK, MessageBoxImage.Warning);

                }
                else
                    MessageBox.Show("Wrócili zanim wyjechali?", "Coś poszło nie tak", MessageBoxButton.OK, MessageBoxImage.Question);
            }
            catch (Exception)
            {
                MessageBox.Show("Błędnie wypełnione daty lub godziny!", "Erroł", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    

        private void spUnits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (CheckBox item in spUnits.Items)
            {
                if (item.TabIndex == spUnits.SelectedIndex)
                    item.IsChecked = true;
            }
        }

        private void btnEquivalentMenu1_Click(object sender, RoutedEventArgs e)
        {
            gAddedUnits.Visibility = Visibility.Visible;
            gAddUnits.Visibility = Visibility.Hidden;
            gRemoveUnits.Visibility = Visibility.Hidden;
            gMainInfo.HorizontalAlignment = HorizontalAlignment.Left;
            gAddedUnits.HorizontalAlignment = HorizontalAlignment.Right;
            gMainInfo.Visibility = Visibility.Visible;
        }

        private void btnEquivalentMenu5_Click(object sender, RoutedEventArgs e)
        {
            gAddedUnits.Visibility = Visibility.Hidden;
            gAddUnits.Visibility = Visibility.Hidden;
            gMainInfo.Visibility = Visibility.Hidden;
            spUnitstoRemove.Items.Clear();
            foreach (var unit in units)
            {
                CheckBox chk = new CheckBox();
                chk.Content = unit;
                //chk.FontSize = 24;
                ScaleTransform scale = new ScaleTransform(1.2, 1.2);
                //chk.RenderTransformOrigin = new Point(0.5, 0.5);
                chk.RenderTransform = scale;
                chk.Padding = new Thickness(5, 0, 0, 0);
                chk.Width = 350;
                chk.VerticalAlignment = VerticalAlignment.Center;
                spUnitstoRemove.Items.Add(chk);
            }
            gRemoveUnits.Visibility = Visibility.Visible;
        }

        private void spUnitstoRemove_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (CheckBox item in spUnitstoRemove.Items)
            {
                if (item.TabIndex == spUnitstoRemove.SelectedIndex)
                    item.IsChecked = true;
            }
        }

        private void btRemoveUnits_Click(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox item in spUnitstoRemove.Items)
            {
                if(item.IsChecked == true)
                {
                    Unit i = (Unit)item.Content;
                    units.Remove(i);   
                    zastep.Remove(i.Rescuer);
                }
            }
            gRemoveUnits.Visibility = Visibility.Hidden;
            gMainInfo.Visibility = Visibility.Visible;
            gAddedUnits.Visibility = Visibility.Visible;
        }

        private void btnEquivalentMenu4_Click(object sender, RoutedEventArgs e)
        {
            gRemoveUnits.Visibility = Visibility.Hidden;
            gMainInfo.Visibility = Visibility.Visible;
            gAddedUnits.Visibility = Visibility.Visible;
            gAddUnits.Visibility = Visibility.Hidden;
            zastep = new List<Druh>();
            units = new List<Unit>();
            dpEquivalentOccurrence.SelectedDate = DateTime.Today;
            dpStartTime.SelectedDate = DateTime.Today;
            dpEndTime.SelectedDate = DateTime.Today;
            tbEndHour.Text = "";
            tbStartHour.Text = "";
            tbStartMinute.Text = "";
            tbEndMinute.Text = "";
            tbRecordNumber.Text = "";
            tbOccurenceName.Text = "";
            dgRescuers.ItemsSource = null;
            // dgRescuers.Items.Clear();
            wniosek = null;
        }

        Wniosek1 wniosek = null;
        private void btnEquivalentMenu3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (Druh item in zastep)
                {
                    item.IleAkcji++;
                }
                int counter = 1;
                foreach (Unit item in units)
                {
                    item.ID = counter;
                    counter++;
                }
                EquivalentApplication application = new EquivalentApplication((DateTime)dpEquivalentOccurrence.SelectedDate, units, tbRecordNumber.Text, tbOccurenceName.Text, (DateTime)dpApplication.SelectedDate);
                wniosek = new Wniosek1(application);
                wniosek.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("Nie wypełniono wszystkich wymaganych pól.", "Erroł", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void btnEquivalentMenu6_Click(object sender, RoutedEventArgs e)
        {
            if(wniosek != null)
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(wniosek, "My First Print Job");
                }
            }
            else
            {
                MessageBox.Show("Najpierw wygeneruj wniosek.", "Erroł", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }

        private void ButtonStartMenu4_Click(object sender, RoutedEventArgs e)
        {
            gINFO.Visibility = Visibility.Visible;
            gStartGrid.Visibility = Visibility.Hidden;
            OSPUrodziny.Visibility = Visibility.Hidden;
            OSPEquivalent.Visibility = Visibility.Hidden;
        }

        private void btInfoQuit_Click(object sender, RoutedEventArgs e)
        {
            gINFO.Visibility = Visibility.Hidden;
            gStartGrid.Visibility = Visibility.Visible;
        }

        private void ButtonMenu4_Click(object sender, RoutedEventArgs e)
        {
            ButtonMenu_Click(this, null);
            gStartGrid.Visibility = Visibility.Hidden;
            OSPUrodziny.Visibility = Visibility.Hidden;
            OSPEquivalent.Visibility = Visibility.Hidden;
            gINFO.Visibility = Visibility.Visible;
        }
    }
}
