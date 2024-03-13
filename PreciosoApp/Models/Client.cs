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
        public string? Name { get; set; }
        public DateTime DOB { get; set; }
        public DateOnly DOB_date { get; set; }
        public int Age { get; set; }
        public string? ContactInfo { get; set; }
        public string? Gender { get; set; }

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
                            client.DOB = reader.GetDateTime("client_dob");
                            client.DOB_date = DateOnly.FromDateTime(client.DOB);
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

        public List<string> GetClientName()
        {
            Database db = new Database();
            List<string> client = new List<string>();

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
                            String Name = reader.GetString("client_name");
                            client.Add(Name);
                        }
                    }
                }

            }

            return client;
        }

        public List<int> GetClientID(String clientName)
        {
            Database db = new Database();
            List<int> client = new List<int>();

            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();

                string query = "SELECT c.client_id, c.client_name,DATE(c.client_dob) AS client_dob, YEAR(CURDATE()) - YEAR(c.client_dob) AS age, " +
                   $"c.client_contactinfo, g.gender FROM tbl_client c LEFT JOIN tbl_gender g ON c.client_gender = g.gender_id WHERE c.client_name = '{clientName}';";


                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int Id = reader.GetInt32("client_id");
                            client.Add(Id);
                        }
                    }
                }

            }

            return client;
        }

        public static List<Client> FilterClientsByName(List<Client> allClients, string searchText)
        {
            return allClients.FindAll(client => client.Name.ToLower().Contains(searchText.ToLower()));
        }

        public void addClient(string lastname, string firstname, DateTime dob, string contactinfo, int gender)
        {

            string name = lastname + ", " + firstname;
            Database db = new Database();
            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();

                string query = "INSERT INTO tbl_client (client_name, client_dob, client_contactinfo, client_gender) VALUES (@Name, @DOB, @ContactInfo, @Gender)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@DOB", dob.Date);
                    cmd.Parameters.AddWithValue("@ContactInfo", contactinfo);
                    cmd.Parameters.AddWithValue("@Gender", gender);

                    cmd.ExecuteNonQuery();
                }
            }
        }



        public void updateClient(int id, string name, DateTimeOffset dob, string contactInfo, int genderId)
        {
            Database db = new Database();
            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = "UPDATE tbl_client SET client_name = @Name, client_dob = @DOB, client_contactinfo = @ContactInfo, " +
                              "client_gender = @GenderID WHERE client_id = @ID;";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@DOB", dob);
                cmd.Parameters.AddWithValue("@ContactInfo", contactInfo);
                cmd.Parameters.AddWithValue("@GenderID", genderId);
                cmd.Parameters.AddWithValue("@ID", id); // Assuming ID is the primary key for therapist

                // Execute the update query
                cmd.ExecuteNonQuery();
            }
        }
    }
}
