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
    /// Логика взаимодействия для ListWindow.xaml
    /// </summary>
    public partial class ListWindow : Window
    {
        public string result;
        public ListWindow()
        {
            InitializeComponent();
        }
        public ListWindow(string format)
        {
            InitializeComponent();
            Result.Text = format.Replace('|', '\n');
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            result = "";
            foreach (string rs in Result.Text.Split('\n'))
            {
                if (result.Length > 0)
                    result += "|";
                result += rs.Trim();
            }
            DialogResult = true;
            this.Close();
        }
    }
}
