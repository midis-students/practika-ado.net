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

        private int id;

        public Employee(int id = -1)
        {
            this.id = id;
            InitializeComponent();

            var pos = Positions.FindAll();

            PositionList.Items.Clear();

            foreach(var item in pos )
            {
                PositionList.Items.Add(item);
            }

            Salary.Content = "-";
            DeleteButton.Visibility = id == -1 ? Visibility.Hidden : Visibility.Visible;
            update_data();
        }

        private void PositionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = sender as ComboBox;
            var item = list.SelectedItem as Positions;

            Salary.Content = item.salary + " $";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (FirstName.Text.Length == 0) return;
            if (LastName.Text.Length == 0) return;
            if (MiddleName.Text.Length == 0) return;

            if (id == -1)
            {
                Employees.Create(FirstName.Text,MiddleName.Text, LastName.Text, ((Positions)PositionList.SelectedItem).id );
            }
            else
            {
                Employees.Update(id,FirstName.Text, MiddleName.Text, LastName.Text, ((Positions)PositionList.SelectedItem).id);
            }
            MessageBox.Show("Сохранено!");
            Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите уволить работягу?",
                                "Уволить?!",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Employees.Delete(id);
                MessageBox.Show("Успешно удалили");
                Close();
            }
        }

        void update_data()
        {
            if (id == -1) return;

            var e = Employees.FindOne(id);

            FirstName.Text = e.first_name;
            LastName.Text = e.last_name;
            MiddleName.Text = e.middle_name;

            PositionList.Text = e.position.name;
            PositionList.SelectedItem = e.position;
            Salary.Content = e.position.salary + " $";

        }
    }
}
