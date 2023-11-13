using DB_Practika.Database;
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

namespace DB_Practika
{
    /// <summary>
    /// Interaction logic for Employee.xaml
    /// </summary>
    public partial class Employee : Window
    {
        public Employee()
        {
            InitializeComponent();

            var pos = Positions.FindAll();

            PositionList.Items.Clear();

            foreach(var item in pos )
            {
                PositionList.Items.Add(item);
            }

            Salary.Content = "-";

        }

        private void PositionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = sender as ComboBox;
            var item = list.SelectedItem as Positions;

            Salary.Content = item.salary + " $";

            Console.WriteLine(item.id);
        }
    }
}
