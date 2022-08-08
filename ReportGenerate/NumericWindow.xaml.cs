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
    /// Логика взаимодействия для NumericWindow.xaml
    /// </summary>
    public partial class NumericWindow : Window
    {
        public string result;
        public NumericWindow()
        {
            InitializeComponent();
            this.TabIndex = 0;
        }
        public NumericWindow(string format)
        {
            InitializeComponent();
            try
            {
                BothVal.Text = Convert.ToDouble(format.Split(new char[] { '|' }, 2)[0]).ToString();
                try
                {
                format = format.Split(new char[] { '|' }, 2)[1];
                }
                catch
                {
                    format = "";
                }
            }
            catch { }
            try
            {
                TopVal.Text = Convert.ToDouble(format.Split(new char[] { '|' }, 2)[0]).ToString();
                try
                {
                    format = format.Split(new char[] { '|' }, 2)[1];
                }
                catch
                {
                    format = "";
                }
            }
            catch { }
            try
            {
                Format.Text = Convert.ToUInt16(format).ToString();
            }
            catch { }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            result = "";
            try
            {
                result += Convert.ToDouble(BothVal.Text).ToString();
            }
            catch
            {
                result += "0";
            }
            finally
            {
                result += '|';
            }
            try
            {
                result += Convert.ToDouble(TopVal.Text).ToString();
            }
            catch
            {
                result += "0";
            }
            finally
            {
                result += '|';
            }
            if (Format.Text.Length>0)
            {
                result += Format.Text;
            }
            else
            {
                result += "0";
            }
            DialogResult = true;
            this.Close();
        }
        /*
        private void Result_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == "\n" || e.Text == "\t")
            {
                e.Handled = true;
                this.TabIndex += 1;
            }
            else if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void BothVal_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == "\n" || e.Text == "\t")
            {
                e.Handled = true;
                this.TabIndex += 1;
            }
            else if (e.Text == "-")
            {
                if (BothVal.Text.Substring(0,1)=="-")
                {
                    BothVal.Text = BothVal.Text.Substring(1);
                }
                else
                {
                    BothVal.Text = "-" + BothVal.Text;
                }
                e.Handled = true;
            }
            else if (e.Text == "." || e.Text==",")
            {
                if (BothVal.Text.Contains(".") || BothVal.Text.Contains(","))
                {
                    e.Handled = true;
                }
            }
            else if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void Val_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == "\n" || e.Text == "\t")
            {
                e.Handled = true;
                this.TabIndex += 1;
            }
            else if (!Char.IsDigit(e.Text, 0) && e.Text != "." && e.Text != "-")
            {
                e.Handled = true;
            }
        }
        */

        private void Format_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
                e.Handled = true;
        }
    }
}
