using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreciosoApp.Models
{
    public class Database
    {
        public MySqlConnection GetCon()
        {
            string mysqlCon = "server=localhost; user=root; database=db_preciosospa3; password=";
            MySqlConnection conn = new MySqlConnection(mysqlCon);
            return conn;
        }

        public MySqlConnection GetOpenConnection()
        {
            Database db = new Database();
            MySqlConnection conn = db.GetCon();
            conn.Open();
            return conn;
        }

        public List<T> ExecuteQuery<T>(string query, Func<MySqlDataReader, T> mapRow)
        {
            using (MySqlConnection conn = GetOpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<T> results = new List<T>();
                        while (reader.Read())
                        {
                            results.Add(mapRow(reader));
                        }
                        return results;
                    }
                }
            }
        }

        public bool TestConnection()
        {
            MySqlConnection conn = GetCon();

            try
            {
                conn.Open();
                conn.Close(); // Close the connection if opened successfully
                return true;
            }
            catch (MySqlException ex)
            {
                // Handle exception or log error message
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}
