using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DB_Practika.Database
{
    internal class EmployHistory
    {

        public int id { get; set; }

        public Employees employe { get; set; }
        public Positions position { get; set; }

        public string start_date { get; set; }
        public string end_date { get; set; }


        public static List<EmployHistory> FindAll()
        {
            var list = new List<EmployHistory>();
            using var connection = SQL.Instance.getConnection();
            var query = @" SELECT * FROM EmployHistory ";
            var command = new SqlCommand(query, connection);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new EmployHistory()
                    {
                        id = (int)reader["id"],
                        employe = Employees.FindOne((int)reader["employee_id"]),
                        position = Positions.FindOne((int)reader["position_id"]),
                        start_date = ((DateTime)reader["start_date"]).ToString(),
                        end_date = ((DateTime)reader["end_date"]).ToString(),
                    });
                }
            }

            return list;
        }
      
        public static List<EmployHistory> FindByEmploye(int emp_id)
        {
            var list = new List<EmployHistory>();
            using var connection = SQL.Instance.getConnection();
            var query = @" SELECT * FROM EmployHistory WHERE employee_id=@id ";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", emp_id);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new EmployHistory()
                    {
                        id = (int)reader["id"],
                        employe = Employees.FindOne((int)reader["employee_id"]),
                        position = Positions.FindOne((int)reader["position_id"]),
                        start_date = ((DateTime)reader["start_date"]).ToString(),
                        end_date = ((DateTime)reader["end_date"]).ToString(),
                    });
                }
            }

            return list;
        }
      
        public static void AddToHistory(int employee_id, int pos_id)
        {
            using var connection = SQL.Instance.getConnection();
            var insertCommand = new SqlCommand("INSERT INTO EmployHistory(employee_id, position_id, start_date) VALUES (@employeeId, @positionId, @startDate)", connection);

            insertCommand.Parameters.AddWithValue("employeeId", employee_id);
            insertCommand.Parameters.AddWithValue("positionId", pos_id);
            insertCommand.Parameters.AddWithValue("startDate", DateTime.Now);
           
            insertCommand.ExecuteNonQuery();

            var updateEmployeeHistory = new SqlCommand("UPDATE EmployHistory SET end_date = @endDate WHERE employee_id = @employeeId AND end_date IS NULL", connection);
            updateEmployeeHistory.Parameters.AddWithValue("@employeeId", employee_id);
            updateEmployeeHistory.Parameters.AddWithValue("@endDate", DateTime.Now);

            updateEmployeeHistory.ExecuteNonQuery();
        }

        public static void InitTable()
        {
            if (SQL.tableExists("EmployHistory")) return;

            using var connection = SQL.Instance.getConnection();
            var query = @"
                CREATE TABLE [EmployHistory](
                    [id] INT PRIMARY KEY IDENTITY(1,1),
                    [employee_id] INT,
                    [position_id] INT,
                    [start_date] DATE,
                    [end_date] DATE,
                    FOREIGN KEY (employee_id) REFERENCES Employees(id) ON DELETE CASCADE,
                    FOREIGN KEY (position_id) REFERENCES Positions(id) ON DELETE CASCADE
                );
            ";
            var command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();

            Console.WriteLine("[EmployHistory] created");
        }
    }
}
