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
    /// Логика взаимодействия для FieldsWindow.xaml
    /// </summary>
    public partial class FieldsWindow : Window
    {
        private readonly List<string> _fields = new List<string>();
        public string Result
        {
            get
            {
                string result="";
                foreach(string field in _fields)
                {
                    if (result.Length > 0)
                        result += '\n';
                    result += field;
                }
                return result;
            }
            set
            {
                if (value.Length > 0)
                {
                    foreach (string line in value.Split('\n'))
                    {
                        _fields.Add(line);
                    }
                }
                else
                {
                    _fields.Clear();
                }
            }
        }

        private void ResetItems()
        {
            Items.Items.Clear();
            foreach (string i in _fields)
            {
                Items.Items.Add(i);
            }
        }

        public FieldsWindow(string fields)
        {
            Result = fields;
            InitializeComponent();
            ResetItems();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            if (Items.SelectedItem != null)
            { 
            _fields.Remove(Items.SelectedItem.ToString());
            }
            ResetItems();
        }

        private void Correct_Click(object sender, RoutedEventArgs e)
        {
            if (Items.SelectedItem != null)
            {
                for (int i = 0; i< _fields.Count; i++)
                {
                    if (_fields[i] == Items.SelectedItem.ToString())
                    {
                        UInt32 num;
                        try
                        {
                            num = Convert.ToUInt32(_fields[i].Split(new char[] { '\\' }, 2)[0]);
                            try
                            {
                                _fields[i] = _fields[i].Split(new char[] { '\\' }, 2)[1].Trim();
                                if (_fields[i].Substring(0, 1) == "D")
                                {
                                    DateWindow f = new DateWindow(_fields[i].Substring(2).Trim())
                                    {
                                        Owner = this
                                    };
                                    f.ShowDialog();
                                    _fields[i] = "D: " + f.result;
                                }
                                else if (_fields[i].Substring(0, 1) == "L")
                                {
                                    ListWindow listWindow = new ListWindow(_fields[i].Substring(2).Trim())
                                    {
                                        Owner = this
                                    };
                                    listWindow.ShowDialog();
                                    _fields[i] = "L: " + listWindow.result;
                                }
                                else if (_fields[i].Substring(0, 1) == "V")
                                {
                                    NumericWindow f = new NumericWindow(_fields[i].Substring(2).Trim())
                                    {
                                        Owner = this
                                    };
                                    f.ShowDialog();
                                    _fields[i] = "V: " + f.result;
                                }
                                else if (_fields[i].Substring(0, 1) == "B")
                                {
                                    _fields[i] = "B";
                                }
                                _fields[i] = num.ToString() + "\\ " + _fields[i];
                            }
                            catch
                            {
                                _fields[i] = "";
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
            ResetItems();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            UInt32 num = Convert.ToUInt32(_fields.Count + 1);
            TypeWindow f = new TypeWindow
            {
                Owner = this
            };
            try { f.ShowDialog(); }
            catch { }
            _fields.Add(num.ToString() + "\\ " + f.result);
            ResetItems();
        }

        private void Items_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Items.SelectedItem != null)
                Correct_Click(new object(), new RoutedEventArgs());
            else
                Add_Click(new object(), new RoutedEventArgs());
        }

        private void Items_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                if (Items.SelectedItem != null)
                    Correct_Click(new object(), new RoutedEventArgs());
                else
                    Add_Click(new object(), new RoutedEventArgs());
            else if (e.Key == Key.Back && Items.SelectedItem != null)
                Del_Click(new object(), new RoutedEventArgs());
        }
    }
}
