using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreciosoApp.Models
{
    internal class TypesQueries
    {
        public List<Gender> GetGenders()
        {
            Database db = new Database();
            List<Gender> genders = new List<Gender>();

            using(MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = "Select * from tbl_gender";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Gender gender= new Gender();
                            gender.Id = reader.GetInt32("gender_id");
                            gender.GenderType = reader.GetString("gender");
                            genders.Add(gender);
                        }
                    }
                }
            }

            return genders;
        }
    }
}
