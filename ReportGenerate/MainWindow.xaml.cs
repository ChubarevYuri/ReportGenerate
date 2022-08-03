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

        private void timerStart()
        {
            _timer = new System.Windows.Threading.DispatcherTimer();
            _timer.Tick += new EventHandler(timerTick);
            _timer.Interval = new TimeSpan(0, 0, 0, 5, 0);
            _timer.Start();
        }

        private void timerTick(object sender, EventArgs e)
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
                        DateTime date = DateStart.DisplayDate.Add(TimeSpan.FromTicks((DateStop.DisplayDate.Subtract(DateStart.DisplayDate).Ticks) / count * i));
                        await Task.Run(() => Gen(i, date, text, f));
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

        private void Gen(int i, DateTime date, string text, System.IO.StreamWriter f)
        {
            try
            {
                text = text.Replace("[N]", i.ToString());
                string formate = "";
                bool startChar = false;
                foreach (char c in text)
                {
                    if (c == '[')
                    {
                        formate = "";
                        startChar = true;
                    }
                    else if (c == ']')
                    {
                        if (startChar)
                        {
                            if (formate.StartsWith("D:"))
                            {
                                string old = "[" + formate + "]";
                                try
                                {
                                    text = text.Replace(old, date.ToString(formate.Substring(2).Trim()));
                                }
                                catch { }
                            }
                            if (formate.StartsWith("V:"))
                            {
                                string old = "[" + formate + "]";
                                Random rnd = new Random();
                                try
                                {
                                    int a = Convert.ToInt32(formate.Substring(2).Trim().Split('_')[0]);
                                    int b = Convert.ToInt32(formate.Substring(2).Trim().Split('_')[1]);
                                    text = text.Replace(old, rnd.Next(a, b + 1).ToString());
                                }
                                catch { }
                            }
                            if (formate.StartsWith("M:"))
                            {
                                string old = "[" + formate + "]";
                                Random rnd = new Random();
                                try
                                {
                                    string[] m = formate.Substring(2).Trim().Split(',');
                                    text = text.Replace(old, m[rnd.Next(m.Length)].Trim());
                                }
                                catch { }
                            }
                        }
                        startChar = false;
                    }
                    else if (startChar)
                            formate += c;
                }
                f.Write("<" + i.ToString() + "\n" + text + "\n>\n");
            }
            catch { }
        }

        private void Count_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void Report_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.IO.File.SetAttributes("~text.t", System.IO.FileAttributes.Normal);
            using (System.IO.StreamWriter f = new System.IO.StreamWriter("~text.t"))
            {
                f.Write(Report.Text);
            }
            System.IO.File.SetAttributes("~text.t", System.IO.FileAttributes.Hidden);
        }
    }
}
