using DB_Practika.Database;
using System;
using System.Windows;


namespace DB_Practika
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var count = Employees.GetAll().Count;
            Console.WriteLine(count);
        }
    }
}
