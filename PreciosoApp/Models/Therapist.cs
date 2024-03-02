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
        public DateTime DOB { get; set; }
        public DateOnly DOB_date { get; set; }
        public string ContactInfo { get; set; }
        public string Sched {  get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }

        public Therapist() { }

        public List<Therapist> GetAllTherapist() 
        {
            Database db = new Database();
            List<Therapist> therapists = new List<Therapist>();

            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();

                string query = "select t.therapist_id, t.name, t.dob, t.contactinfo, t.schedule, g.gender, s.status, ty.type " +
                                 "from tbl_therapist t " +
                                 "left join tbl_gender g on t.gender = g.gender_id " +
                                 "left join tbl_therapist_status s on t.status = s.status_id " +
                                 "left join tbl_therapist_type ty on t.type = ty.type_id;";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using(MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Therapist therapist = new Therapist();
                            therapist.Id = reader.GetInt32("therapist_id");
                            therapist.Name = reader.GetString("name");
                            therapist.DOB = reader.GetDateTime("dob");
                            therapist.DOB_date = DateOnly.FromDateTime(reader.GetDateTime("dob"));
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

        public void UpdateTherapist(int id, string name, DateTimeOffset dob, string contactInfo, string sched, int genderId, int statusId, int typeId)
        {
            Database db = new Database();
            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();


                string query = "UPDATE tbl_therapist SET name = @Name, dob = @DOB, contactinfo = @ContactInfo, " +
                               "schedule = @Sched, gender = @GenderID, status = @StatusID, type = @TypeID " +
                               "WHERE therapist_id = @TherapistID;";


                MySqlCommand cmd = new MySqlCommand(query, conn);

                // Add parameters with actual values
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@DOB", dob);
                cmd.Parameters.AddWithValue("@ContactInfo", contactInfo);
                cmd.Parameters.AddWithValue("@Sched", sched);
                cmd.Parameters.AddWithValue("@GenderID", genderId);
                cmd.Parameters.AddWithValue("@StatusID", statusId);
                cmd.Parameters.AddWithValue("@TypeID", typeId);
                cmd.Parameters.AddWithValue("@TherapistID", id); // Assuming ID is the primary key for therapist

                // Execute the update query
                cmd.ExecuteNonQuery();
            }
        }

        public void AddTherapist(string name, DateTime dob, string contactInfo, string sched, int genderId, int statusId, int typeId)
        {
            Database db = new Database();

            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = "insert into tbl_therapist(name, dob, contactinfo, schedule, gender, status, type) values (@name, @dob, @ContactInfo, @schedule, @gender, @status, @type)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@dob", dob);
                    cmd.Parameters.AddWithValue("@ContactInfo", contactInfo);
                    cmd.Parameters.AddWithValue("@schedule", sched);
                    cmd.Parameters.AddWithValue("@gender", genderId);
                    cmd.Parameters.AddWithValue("@status", statusId);
                    cmd.Parameters.AddWithValue("@type", typeId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<string> GetTherapistName()
        {
            Database db = new Database();
            List<string> therapist = new List<string>();

            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();

                string query = "select t.therapist_id, t.name, t.dob, t.contactinfo, t.schedule, g.gender, s.status, ty.type " +
                               "from tbl_therapist t " +
                               "left join tbl_gender g on t.gender = g.gender_id " +
                               "left join tbl_therapist_status s on t.status = s.status_id " +
                               "left join tbl_therapist_type ty on t.type = ty.type_id;"; 
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            String Name = reader.GetString("name");
                            therapist.Add(Name);
                        }
                    }
                }

            }
            return therapist;
        }

        public List<int> GetTherapistID(string therapistName)
        {
            Database db = new Database();
            List<int> therapist = new List<int>();

            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();

                string query = "select t.therapist_id, t.name, t.dob, t.contactinfo, t.schedule, g.gender, s.status, ty.type " +
                               "from tbl_therapist t " +
                               "left join tbl_gender g on t.gender = g.gender_id " +
                               "left join tbl_therapist_status s on t.status = s.status_id " +
                               $"left join tbl_therapist_type ty on t.type = ty.type_id WHERE t.name = '{therapistName}';";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int Id = reader.GetInt32("therapist_id");
                            therapist.Add(Id);
                        }
                    }
                }

            }
            return therapist;
        }
    }
}
