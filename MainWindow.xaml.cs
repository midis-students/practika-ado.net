
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DB_Practika
{


    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            update_positions();
            update_employees();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var tab = ((TabItem)TabControl.SelectedItem).Name;

            Window window = null;

            switch (tab)
            {
                case "Employee":
                    window = new Employee();
                    break;
                case "Positions":
                    window = new Position();
                    break;
                case "EmployeeHistory": break;
                default: MessageBox.Show("TabControl.SelectedItem = " + tab); break;
            }

            if (window != null)
            {
                window.Show();
            }
        }

        void update_positions()
        {
            var pos = Database.Positions.FindAll();

            PositionsData.Items.Clear();

            foreach (var position in pos)
            {
                PositionsData.Items.Add(position);
            }
        }

        void update_employees()
        {
            var list = Database.Employees.FindAll();

            EmployeesList.Items.Clear();

            foreach (var employee in list)
            {
                EmployeesList.Items.Add(employee);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var tab = ((TabItem)TabControl.SelectedItem).Name;

            switch (tab)
            {
                case "Employee":
                    update_employees();
                    break;
                case "Positions":
                    update_positions();
                    break;
                case "EmployeeHistory": break;
                default: MessageBox.Show("TabControl.SelectedItem = " + tab); break;
            }
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            var tab = ((TabItem)TabControl.SelectedItem).Name;

            Window window = null;

            switch (tab)
            {
                case "Employee":
                    window = new Employee(((Database.Employees)row.Item).id);
                    break;
                case "Positions":
                    window = new Position(((Database.Positions)row.Item).id);
                    break;
                case "EmployeeHistory": break;
                default: MessageBox.Show("TabControl.SelectedItem = " + tab); break;
            }

            if (window != null)
            {
                window.Show();
            }
        }
    }
}
