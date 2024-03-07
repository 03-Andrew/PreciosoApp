using DynamicData;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreciosoApp.Models
{
    public class Promos
    {
        public int promoID { get; set; }
        public string promoName { get; set; }
        public float promoCost { get; set; }

        public List<Promos> GetPromos()
        {
            Database db = new Database();
            List<Promos> promos = new List<Promos>();

            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();

                string query = "SELECT promo_id, promo, price FROM tbl_promo;";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Promos prmo = new Promos();
                            prmo.promoID = reader.GetInt32("promo_id");
                            prmo.promoName = reader.GetString("promo");
                            prmo.promoCost = reader.GetFloat("price");
                            promos.Add(prmo);
                        }
                    }
                }
            }
            return promos;
        }

        public List<string> GetPromoNames()
        {
            Database db = new Database();
            List<string> promos = new List<string>();

            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();

                string query = "SELECT promo_id, promo, price FROM tbl_promo;";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string promoName = reader.GetString("promo");
                            promos.Add(promoName);
                        }
                    }
                }
            }
            return promos;
        }
    }
}
