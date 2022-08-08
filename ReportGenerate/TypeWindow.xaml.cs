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
    /// Логика взаимодействия для TypeWindow.xaml
    /// </summary>
    public partial class TypeWindow : Window
    {
        public string result;
        public TypeWindow()
        {
            InitializeComponent();
        }

        private void Mass_Click(object sender, RoutedEventArgs e)
        {
            ListWindow listWindow = new ListWindow
            {
                Owner = this
            };
            listWindow.ShowDialog();
            result = "L: " + listWindow.result;
            this.Close();
        }

        private void Date_Click(object sender, RoutedEventArgs e)
        {
            DateWindow f = new DateWindow
            {
                Owner = this
            };
            f.ShowDialog();
            result = "D: " + f.result;
            this.Close();
        }

        private void Num_Click(object sender, RoutedEventArgs e)
        {
            NumericWindow f = new NumericWindow
            {
                Owner = this
            };
            f.ShowDialog();
            result = "V: " + f.result;
            this.Close();
        }

        private void Bool_Click(object sender, RoutedEventArgs e)
        {
            result = "B";
            this.Close();
        }
    }
}
