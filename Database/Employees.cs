using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DB_Practika.Database
{
    internal class Employees
    {

        public int id { get; set; }
        public string first_name { get; set; }

        public string middle_name { get; set; }
        public string last_name { get; set; }
        public Positions position { get; set; }

        public string short_name
        {
            get
            {
                return $"{last_name} {first_name[0]}. {middle_name[0]}.";
            }
        }

        public static List<Employees> FindAll()
        {
            var list = new List<Employees>();
            using var connection = SQL.Instance.getConnection();
            var query = @" SELECT * FROM Employees";
            var command = new SqlCommand(query, connection);

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
                        position = Positions.FindOne((int)reader["position"]),
                    });
                }
            }
            return list;
        }

        public static Employees FindOne(int id)
        {
            using var connection = SQL.Instance.getConnection();
            var query = @" SELECT * FROM Employees WHERE id=@id";
            var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", id);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                reader.Read();

                return new Employees()
                {
                    id = (int)reader["id"],
                    first_name = (string)reader["first_name"],
                    last_name = (string)reader["last_name"],
                    middle_name = (string)reader["middle_name"],
                    position = Positions.FindOne((int)reader["position"]),
                };

            }
        }

        public static void Delete(int id)
        {
            using var connection = SQL.Instance.getConnection();
            var query = @" DELETE FROM Employees WHERE id=@id";
            var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        public static int Create(string first_name, string middle_name, string last_name, int position)
        {
            using var connection = SQL.Instance.getConnection();
            var query = @" INSERT INTO Employees(first_name,last_name,middle_name,position) VALUES ( @first_name, @last_name, @middle_name,@pos); SELECT SCOPE_IDENTITY();";
            var command = new SqlCommand(query, connection);

            command.Parameters.AddRange(new[]{
                new SqlParameter("@first_name", first_name){ SqlDbType = SqlDbType.NVarChar,Size = 255 },
                new SqlParameter("@middle_name", middle_name){ SqlDbType = SqlDbType.NVarChar,Size = 255 },
                new SqlParameter("@last_name", last_name){ SqlDbType = SqlDbType.NVarChar,Size = 255  },
                new SqlParameter("@pos", position){ SqlDbType = SqlDbType.Int  },
            });

            return Convert.ToInt32(command.ExecuteScalar());
        }
        public static void Update(int id, string first_name, string middle_name, string last_name, int position)
        {
            using var connection = SQL.Instance.getConnection();
            var query = @" UPDATE Employees SET first_name = @first_name,last_name = @last_name, middle_name = @middle_name,position = @pos WHERE id=@id ";
            var command = new SqlCommand(query, connection);

            command.Parameters.AddRange(new[]{
                new SqlParameter("@first_name", first_name){ SqlDbType = SqlDbType.NVarChar,Size = 255 },
                new SqlParameter("@middle_name", middle_name){ SqlDbType = SqlDbType.NVarChar,Size = 255 },
                new SqlParameter("@last_name", last_name){ SqlDbType = SqlDbType.NVarChar,Size = 255  },
                new SqlParameter("@pos", position){ SqlDbType = SqlDbType.Int  },
                new SqlParameter("@id", id){ SqlDbType = SqlDbType.Int  },
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
