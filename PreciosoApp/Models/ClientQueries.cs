using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreciosoApp.Models
{
    internal class ClientQueries
    {   
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
            using(MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = "UPDATE tbl_therapist SET name = @Name, dob = @DOB, contactinfo = @ContactInfo, " +
                              "gender = @GenderID, status = @StatusID, type = @TypeID " +
                              "WHERE client_id = @ID;";

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
