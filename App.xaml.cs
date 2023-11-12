
using DB_Practika.Database;
using System;
using System.Windows;

namespace DB_Practika
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
       public App()
        {
            Console.WriteLine( SQL.Instance.getConnection().Database );
        }
    }
}
