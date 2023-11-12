using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DB_Practika.Database
{
    abstract class Entity
    {
        public void CreateTable() {

            var type = GetType();
            Console.WriteLine($"Looking at {type.Name}");

            var fields = new List<string>();

            foreach (var prop in type.GetProperties())
            {
                var attrs = (EntityField[])prop.GetCustomAttributes
                    (typeof(EntityField), false);
                foreach (var attr in attrs)
                {
                    var field = attr.type.ToType();
                    if(attr.size > 0)
                    {
                        field += $"({attr.size})";
                    }
                    field += " " + attr.field;
                    fields.Add($"[{prop.Name}] {field}");
                }
            }

            var query = $"CREATE TABLE [{type.Name}]({(String.Join(",", fields))});";
            using var connection = SQL.Instance.getConnection();
            var command = new SqlCommand( query, connection );
            command.ExecuteNonQuery();

        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class EntityField : Attribute { 
        
        public SqlDbType type { get; set; }
        public string field { get; set; }
        public int size { get; set; }

        public EntityField(SqlDbType type)
        {
            this.type = type;
            this.field = field;
            this.size = size;
        }

   
    }
}
