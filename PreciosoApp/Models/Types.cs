using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreciosoApp.Models
{
    public class TherapistTypes
    {
        public int Id { get; set; }
        public string TypeName { get; set; }

        public List<TherapistTypes> GetTherapistTypes()
        {
            List<TherapistTypes> types = new List<TherapistTypes>();
            Database db = new Database();

            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = "select * from tbl_therapist_type";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TherapistTypes type = new TherapistTypes();
                            type.Id = reader.GetInt32("type_id");
                            type.TypeName = reader.GetString("type");

                            types.Add(type);
                        }
                    }
                }
                return types;
            }
        }
    }

    public class Gender
    {
        public int Id { get; set; }
        public string GenderType { get; set; }

        private static readonly Dictionary<string, int> Genders = new Dictionary<string, int>();
        Database db = new Database();
        public List<Gender> GetGenders()
        {
            List<Gender> types = new List<Gender>();


            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = "select * from tbl_gender";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Gender type = new Gender();
                            type.Id = reader.GetInt32("gender_id");
                            type.GenderType = reader.GetString("gender");

                            types.Add(type);
                        }
                    }
                }
                return types;
            }
        }

    }
    public class TherapistStatus
    {
        public int Id { get; set; }
        public string StatusName { get; set; }
        Database db = new Database();

        public List<TherapistStatus> GetTStatus()
        {
            List<TherapistStatus> types = new List<TherapistStatus>();


            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = "select * from tbl_therapist_status";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TherapistStatus type = new TherapistStatus();
                            type.Id = reader.GetInt32("status_id");
                            type.StatusName = reader.GetString("status");

                            types.Add(type);
                        }
                    }
                }
                return types;
            }
        }
    }
}
