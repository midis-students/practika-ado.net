using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DB_Practika.Database
{
    internal class Employees
    {

        public int id { get; private set; }
        public string first_name { get; set; }
 
        public string middle_name { get; set; }
        public string last_name { get; set; }
        public Positions position { get; set; }

        public static List<Employees> GetAll()
        {
            var list = new List<Employees>();

            return list;
        }

        public static void Create(string first_name, string middle_name, string last_name)
        {
            using var connection = SQL.Instance.getConnection();
            var query = @" INSERT INTO Employees(first_name,last_name,middle_name) VALUES ( @first_name, @last_name, @middle_name);";
            var command = new SqlCommand(query, connection);

            command.Parameters.AddRange(new[]{
                new SqlParameter("@first_name", first_name){ SqlDbType = SqlDbType.NVarChar,Size = 255 },
                new SqlParameter("@middle_name", middle_name){ SqlDbType = SqlDbType.NVarChar,Size = 255 },
                new SqlParameter("@last_name", last_name){ SqlDbType = SqlDbType.NVarChar,Size = 255  },
            });

            command.ExecuteNonQuery();

        }

        public static void InitTable()
        {
            if (SQL.tableExists("Employees")) return;

            using var connection = SQL.Instance.getConnection();
            var query = @"
                CREATE TABLE [Employees](
                    [id] INT PRIMARY KEY IDENTITY(1,1),
                    [first_name] NVARCHAR(50),
                    [middle_name] NVARCHAR(50),
                    [last_name] NVARCHAR(50),
                    [position] INT NOT NULL,
                    FOREIGN KEY (position) REFERENCES Positions(id)
                );
            ";
            var command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();

            Console.WriteLine("[Employess] created");
        }
    }
}
