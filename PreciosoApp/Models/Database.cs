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
            string mysqlCon = "server=localhost; user=root; database=db_preciosospa2; password=";
            MySqlConnection conn = new MySqlConnection(mysqlCon);
            return conn;
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
