using DB_Practika.Database;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DB_Practika
{
    /// <summary>
    /// Interaction logic for Position.xaml
    /// </summary>
    public partial class Position : Window
    {
        private int id;
        public Position(int id = -1)
        {
            InitializeComponent();
            this.id = id;
            update_data();
            DeleteButton.Visibility = id == -1 ? Visibility.Hidden : Visibility.Visible;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Name.Text.Length == 0) return;
            if (Salary.Text.Length == 0) return;

            if (id == -1)
            {
                Positions.Create(Name.Text, int.Parse(Salary.Text));
                MessageBox.Show("Сохранено!");
                Close();
            }
        }

        void update_data()
        {
            if (id == -1) return;

            var pos = Positions.FindOne(id);

            Name.Text = pos.name;
            Salary.Text = pos.salary.ToString();

            var list = Positions.FindEmployees(id);

            EmployeesList.Items.Clear();

            foreach (var item in list)
            {
                EmployeesList.Items.Add(item);
            }

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(EmployeesList.Items.Count > 0)
            {

                MessageBox.Show("Нельзя удалить должность пока есть сотрудники с этой должностью");
                return; 
            }
            if (MessageBox.Show("Вы точно хотите удалить должность?",
                                "Да?",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Positions.Delete(id);
                MessageBox.Show("Успешно удалили");
                Close();
            }
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            var window = new Employee(((Employees)row.Item).id);
            window.Show();
        }
    }
}
