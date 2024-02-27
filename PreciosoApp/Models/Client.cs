using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreciosoApp.Models; 

namespace PreciosoApp.Models
{
    public class Client
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string DOB { get; set; }
        public int Age { get; set; }
        public string ContactInfo { get; set; }
        public string Gender { get; set; }

        public Client() { }

        public List<Client> GetAllClients()
        {
            Database db = new Database();   
            List<Client> clients = new List<Client>();

            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();

                string query = "SELECT c.client_id, c.client_name,DATE(c.client_dob) AS client_dob, YEAR(CURDATE()) - YEAR(c.client_dob) AS age, " +
                    "c.client_contactinfo, g.gender FROM tbl_client c LEFT JOIN tbl_gender g ON c.client_gender = g.gender_id;";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Client client = new Client();
                            client.Id = reader.GetInt32("client_id");
                            client.Name = reader.GetString("client_name");
                            client.DOB = reader.GetDateTime("client_dob").ToString().Substring(0,10);
                            client.Age = reader.GetInt32("age");
                            client.ContactInfo = reader.GetString("client_contactinfo");
                            client.Gender = reader.GetString("gender");
                            clients.Add(client);
                        }
                    }
                }

            }
            return clients;
        }
        public static List<Client> FilterClientsByName(List<Client> allClients, string searchText)
        {
            return allClients.FindAll(client => client.Name.ToLower().Contains(searchText.ToLower()));
        }
    }
}
