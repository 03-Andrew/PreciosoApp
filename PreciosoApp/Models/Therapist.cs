using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace PreciosoApp.Models
{
    public class Therapist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DOB { get; set; }
        public string ContactInfo { get; set; }
        public string Sched {  get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }

        public Therapist() { }

        public List<Therapist> getAllTheraist() 
        {
            Database db = new Database();
            List<Therapist> therapists = new List<Therapist>();

            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();

                string query = "select t.therapist_id, t.name, t.dob, t.contactinfo, t.schedule, g.gender, s.status, ty.type" +
                    "from tbl_therapist t left join tbl_gender g on t.gender = g.gender_id left join tbl_therapist_status s on " +
                    "t.status = s.status_id left join tbl_therapist_type ty on t.type = ty.type_id;";

                using(MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using(MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Therapist therapist = new Therapist();
                            therapist.Id = reader.GetInt32("therapist_id");
                            therapist.Name = reader.GetString("name");
                            therapist.DOB = reader.GetString("DOB");
                            therapist.ContactInfo = reader.GetString("contactinfo");
                            therapist.Sched = reader.GetString("schedule");
                            therapist.Gender = reader.GetString("gender");
                            therapist.Status = reader.GetString("status");
                            therapist.Type = reader.GetString("type");
                            therapists.Add(therapist);
                        }
                    }
                }
            }
            return therapists;
        }

    }
}
