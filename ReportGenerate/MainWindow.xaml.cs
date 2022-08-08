using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ReportGenerate
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string filePath = "";
        private bool wait = false;
        private DateTime starttime;
        System.Windows.Threading.DispatcherTimer _timer = null;

#pragma warning disable IDE1006 // Стили именования
        private void timerStart()
#pragma warning restore IDE1006 // Стили именования
        {
            _timer = new System.Windows.Threading.DispatcherTimer();
            _timer.Tick += new EventHandler(timerTick);
            _timer.Interval = new TimeSpan(0, 0, 0, 5, 0);
            _timer.Start();
        }

#pragma warning disable IDE1006 // Стили именования
        private void timerTick(object sender, EventArgs e)
#pragma warning restore IDE1006 // Стили именования
        {
            TBtimer.Text = "";
            _timer.Stop();
            _timer = null;
        }

        public MainWindow()
        {
            InitializeComponent();
            DateStart.DisplayDate = DateTime.Now.AddDays(-7);
            DateStop.DisplayDate = DateTime.Now;
            try
            {
                using (System.IO.StreamReader f = new System.IO.StreamReader("~text.t"))
                {
                        Report.Text = f.ReadToEnd();
                }
                System.IO.File.SetAttributes("~text.t", System.IO.FileAttributes.Hidden);
            }
            catch { }
            try
            {
                using (System.IO.StreamReader f = new System.IO.StreamReader("~field.t"))
                {
                    Fields.Text = f.ReadToEnd();
                }
                System.IO.File.SetAttributes("~field.t", System.IO.FileAttributes.Hidden);
            }
            catch { }
        }

        private void Path_Click(object sender, RoutedEventArgs e)
        {
            if (!wait)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                if (saveFileDialog.ShowDialog() == true)
                    filePath = saveFileDialog.FileName;
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (filePath.Length < 1)
            {
                Path_Click(new object(), new RoutedEventArgs());
            }
            if (!wait && filePath.Length > 0)
            {
                starttime = DateTime.Now;
                wait = true;
                PB.Minimum = 0;
                PB.Value = 0;
                PB.Maximum = 1;
                PB.Visibility = Visibility.Visible;
                int count;
                try { count = Convert.ToInt32(Count.Text); }
                catch { count = 0; }
                try
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(filePath);
                    f.Delete();
                }
                catch { }
                PB.Maximum = count;
                using (System.IO.StreamWriter f = new System.IO.StreamWriter(filePath, true))
                {
                    for (int i = 1; i <= count; i++)
                    {
                        string text = Report.Text;
                        string ff = Fields.Text.ToString().Trim();
                        DateTime date = DateStart.DisplayDate.Add(TimeSpan.FromTicks((DateStop.DisplayDate.Subtract(DateStart.DisplayDate).Ticks) / count * i));
                        await Task.Run(() => Gen(i, date, text, ff, f));
                        PB.Value = i;
                    }
                }
                wait = false;
                PB.Visibility = Visibility.Collapsed;
                TimeSpan t = DateTime.Now - starttime;
                TBtimer.Text = Math.Floor(t.TotalMinutes).ToString() + t.Seconds.ToString(":00") + "." + t.Milliseconds.ToString("000 сек");
                timerStart();
            }
        }

        private void Gen(int i, DateTime date, string text, string fields, System.IO.StreamWriter f)
        {
            try
            {
                text = text.Replace("[N]", i.ToString());
                foreach (string ff in fields.Split('\n'))
                {
                    try
                    {
                        string field = ff;
                        string num = "[" + field.Split(new[] { '\\'},2)[0].Trim() + "]";
                        field = field.Split(new[] { '\\' }, 2)[1].Trim();
                                Random rnd = new Random();
                        switch (field.Split(new[] { ':' }, 2)[0].Trim())
                        {
                            case "D":
                                field = date.ToString(field.Split(new[] { ':' }, 2)[1].Trim());
                                break;
                            case "V":
                                field = field.Split(new[] { ':' }, 2)[1].Trim();
                                int cou = Convert.ToInt32(field.Split(new[] { '|' })[2].Trim());
                                double min = Convert.ToInt32(field.Split(new[] { '|' })[0].Trim());
                                double max = Convert.ToInt32(field.Split(new[] { '|' })[1].Trim());
                                double v = rnd.Next((int)min, (int)max) + rnd.NextDouble();
                                if (v < min)
                                    v = min;
                                if (v > max)
                                    v = max;
                                string format = (cou > 0 ? "#0.": "#0");
                                for (int j=0; j<cou; j++)
                                    format += "0";
                                field = v.ToString(format);
                                break;
                            case "L":
                                field = field.Split(new[] { ':' }, 2)[1].Trim().Split('|')[rnd.Next(field.Split(new[] { ':' }, 2)[1].Trim().Split('|').Length)];
                                break;
                            case "B":
                                field = (rnd.Next(2) == 0 ? "True" : "False");
                                break;
                            default:
                                field = "";
                                break;
                        }
                        text = text.Replace(num, field);
                    }
                    catch { }
                }
                f.Write("<" + i.ToString() + "\n" + text + "\n>\n");
            }
            catch { }
        }

        private void Count_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0) && e.Text[0] == ' ')
            {
                e.Handled = true;
            }
        }

        private void Report_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                System.IO.File.SetAttributes("~text.t", System.IO.FileAttributes.Normal);
            }
            catch { }
            using (System.IO.StreamWriter f = new System.IO.StreamWriter("~text.t"))
            {
                f.Write(Report.Text);
            }
            System.IO.File.SetAttributes("~text.t", System.IO.FileAttributes.Hidden);
        }

        private void Fields_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                System.IO.File.SetAttributes("~field.t", System.IO.FileAttributes.Normal);
            }
            catch { }
            using (System.IO.StreamWriter f = new System.IO.StreamWriter("~field.t"))
            {
                f.Write(Fields.Text);
            }
            System.IO.File.SetAttributes("~field.t", System.IO.FileAttributes.Hidden);
        }

        private void Fields_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FieldsOpen();
        }

        private void FieldsOpen()
        {
            FieldsWindow f = new FieldsWindow(Fields.Text)
            {
                Owner = this
            };
            if (f.ShowDialog() == true)
                Fields.Text = f.Result;
        }
    }
}
