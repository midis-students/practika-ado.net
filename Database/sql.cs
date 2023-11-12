using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

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

            Entity[] entities = { new Employees() };

            Console.WriteLine($"Start {entities.Length} entities sync");

            foreach (var entity in entities)
            {
                if (!tableExists(entity.GetType().Name))
                {
                    entity.CreateTable();
                }
                else
                {
                    Console.WriteLine($"{entity.GetType().Name} table exists");
                }
            }

        }

        public static bool tableExists(string tableName)
        {
            using var connection = Instance.getConnection();
            var query = @"SELECT COUNT(*) FROM sys.objects WHERE object_id = OBJECT_ID(@table) AND type in (N'U')";
            var command = new SqlCommand(query, connection);
            command.Parameters.Add(new SqlParameter("@table", tableName));
            var result = command.ExecuteScalar();
            return (int)result == 1;

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

    public static class TypeExtensions
    {
        public static object GetDefault(this Type t)
        {
            Func<object> f = GetDefault<object>; // quick hack to get generic method
            return f.Method.GetGenericMethodDefinition().MakeGenericMethod(t).Invoke(null, null);
        }

        private static T GetDefault<T>()
        {
            return default(T);
        }
    }

    public static class SqlExtensions
    {
        private static readonly IDictionary<SqlDbType, String> TypeMap = new Dictionary<SqlDbType, String>
        {
            { SqlDbType.Int, "INT"},
            { SqlDbType.NVarChar,"NVARCHAR" },
            // add the rest here
        };

        public static String ToType(this SqlDbType type)
        {
            if (TypeMap.ContainsKey(type))
            {
                return TypeMap[type];
            }

            throw new ArgumentException($"{type} is not a supported");
        }
    }
}


