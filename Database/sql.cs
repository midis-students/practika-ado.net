using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace DB_Practika.Database
{
    public class SQL
    {
        public static SQL Instance;
        public readonly string connectionString;

        static SQL()
        {
            Console.WriteLine("Creating SQL instance");
            new SQL();
        }

        public SQL()
        {

            var config = new ConfigurationBuilder().AddJsonFile(Directory.GetCurrentDirectory() + "/app-settings.json").Build();
            var str = config["Database:uri"];

            if (config == null || str == null)
            {
                throw new Exception("Can't open app-settings.json");
            }

            connectionString = str;

            Instance = this;

            Console.WriteLine($"Start entities sync");

            Employees.InitTable();
            Positions.InitTable();

        }

        public static bool tableExists(string tableName)
        {
            using var connection = Instance.getConnection();
            var query = @"SELECT COUNT(*) FROM sys.objects WHERE object_id = OBJECT_ID(@table) AND type in (N'U')";
            var command = new SqlCommand(query, connection);
            command.Parameters.Add(new SqlParameter("@table", tableName));
            var result = (int)command.ExecuteScalar();
            return result == 1;

        }

        public SqlConnection getConnection()
        {
            var connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return connection;
        }

    }
}


