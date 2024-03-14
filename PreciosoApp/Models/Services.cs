using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreciosoApp.Models
{
    public class Services
    {
        public int servID { get; set; }
        public string servName { get; set; }
        public float servCost { get; set; }
        public float servComm { get; set; }
        public string servType { get; set; }

        public List<Services> GetServices()
        {
            Database db = new Database();
            List<Services> services = new List<Services>();

            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();

                string query = "SELECT s.service_id, s.service_name, s.service_price, c.rate, st.type " +
                    "FROM tbl_service s JOIN tbl_commission_rate c ON s.commission_rate = c.rate_id " +
                    "LEFT JOIN tbl_service_type st ON s.service_type = st.type_id;";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Services serv = new Services();
                            serv.servID = reader.GetInt32("service_id");
                            serv.servName = reader.GetString("service_name");
                            serv.servCost = reader.GetFloat("service_price");
                            serv.servComm = reader.GetFloat("rate");
                            serv.servType = reader.GetString("type");
                            services.Add(serv);
                        }
                    }
                }

            }
            return services;
        }

        public List<string> GetServicesName()
        {
            Database db = new Database();
            List<string> services = new List<string>();

            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();

                string query = "SELECT service_name FROM tbl_service;";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string servName = reader.GetString("service_name");
                            services.Add(servName);
                        }
                    }
                }
            }
            return services;
        }

        public void addClient(string serviceName, float servicePrice, int cRate, int sType)
        {
            Database db = new Database();
            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();

                string query = "INSERT INTO `tbl_service` (`service_name`, `service_price`, `commission_rate`, `service_type`) " +
                               "VALUES (@Name, @Price, @Rate, @Type) ";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", serviceName);
                    cmd.Parameters.AddWithValue("@Price", servicePrice);
                    cmd.Parameters.AddWithValue("@Rate", cRate);
                    cmd.Parameters.AddWithValue("@Type", sType);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
