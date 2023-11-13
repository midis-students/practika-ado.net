using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DB_Practika.Database
{
    internal class Positions
    {

        public int id { get; private set; }

        public string name { get; private set; }
        public int salary { get; private set; }

        public int employees_count { get; private set; }


        public static List<Positions> FindAll()
        {
            var list = new List<Positions>();
            using var connection = SQL.Instance.getConnection();
            var query = @" SELECT P.*,COUNT(E.id) as employes_count FROM Positions P LEFT JOIN Employees E ON E.position = P.id GROUP BY P.id, P.name, P.salary;";
            var command = new SqlCommand(query, connection);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Positions()
                    {
                        id = (int)reader["id"],
                        name = (string)reader["name"],
                        salary = (int)reader["salary"],
                        employees_count = (int)reader["employes_count"]
                    });
                }
            }

            return list;
        }
        public static void Delete(int id)
        {
            using var connection = SQL.Instance.getConnection();
            var query = @" DELETE FROM Positions WHERE id=@id";
            var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
        public static List<Employees> FindEmployees(int pos_id)
        {
            var list = new List<Employees>();
            using var connection = SQL.Instance.getConnection();
            var query = @" SELECT * FROM Employees WHERE position=@pos_id";
            var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@pos_id", pos_id);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Employees()
                    {
                        id = (int)reader["id"],
                        first_name = (string)reader["first_name"],
                        last_name = (string)reader["last_name"],
                        middle_name = (string)reader["middle_name"],
                    });
                }
            }

            return list;
        }

        public static Positions FindOne(int id)
        {
            using var connection = SQL.Instance.getConnection();
            var query = @" SELECT * FROM Positions WHERE id=@id";
            var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", id);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                reader.Read();

                return new Positions()
                {
                    id = (int)reader["id"],
                    name = (string)reader["name"],
                    salary = (int)reader["salary"],
                };

            }
        }

        public static void Create(string name, int salary)
        {
            using var connection = SQL.Instance.getConnection();
            var query = @" INSERT INTO Positions(name,salary) VALUES ( @name, @salary);";
            var command = new SqlCommand(query, connection);

            command.Parameters.AddRange(new[]{
                new SqlParameter("@name", name){ SqlDbType = SqlDbType.NVarChar,Size = 50 },
                new SqlParameter("@salary", salary){ SqlDbType = SqlDbType.Int},
            });

            command.ExecuteNonQuery();
        }

        public static void InitTable()
        {
            if (SQL.tableExists("Positions")) return;

            using var connection = SQL.Instance.getConnection();
            var query = @"
                CREATE TABLE [Positions](
                    [id] INT PRIMARY KEY IDENTITY(1,1),
                    [name] NVARCHAR(50),
                    [salary] INT NOT NULL
                );
            ";
            var command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();

            Console.WriteLine("[Positions] created");
        }
    }
}
