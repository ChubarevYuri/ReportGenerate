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

namespace ReportGenerate
{
    /// <summary>
    /// Логика взаимодействия для DateWindow.xaml
    /// </summary>
    public partial class DateWindow : Window
    {
        public string result;
        public DateWindow()
        {
            InitializeComponent();
        }
        public DateWindow(string format)
        {
            InitializeComponent();
            Result.Text = format;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            result = Result.Text;
            DialogResult = true;
            this.Close();
        }
    }
}
