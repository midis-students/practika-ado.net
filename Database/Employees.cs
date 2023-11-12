using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DB_Practika.Database
{
    internal class Employees : Entity
    {

        [EntityField(SqlDbType.Int, field = "IDENTITY")]
        public int id { get; private set; }
        [EntityField(SqlDbType.NVarChar, size = 255)]
        public string first_name { get; set; }
        [EntityField(SqlDbType.NVarChar, size = 255)]
        public string middle_name { get; set; }
        [EntityField(SqlDbType.NVarChar, size = 255)]
        public string last_name { get; set; }

        public static List<Employees> GetAll()
        {
            var list = new List<Employees>();

            Console.WriteLine(SqlDbType.NVarChar.ToType());

            return list;
        }

        public static void Create(string first_name, string middle_name, string last_name)
        {
            using var connection = SQL.Instance.getConnection();
            var query = @" INSERT INTO Employees(first_name,last_name,middle_name) VALUES ( @first_name, @last_name, @middle_name);";
            var command = new SqlCommand(query, connection);

            command.Parameters.AddRange(new[]{
                new SqlParameter("@first_name", first_name){ SqlDbType = System.Data.SqlDbType.NVarChar,Size = 255 },
                new SqlParameter("@middle_name", middle_name){ SqlDbType = System.Data.SqlDbType.NVarChar,Size = 255 },
                new SqlParameter("@last_name", last_name){ SqlDbType = System.Data.SqlDbType.NVarChar,Size = 255  },
            });

            command.ExecuteNonQuery();

        }
    }
}
