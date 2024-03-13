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
        public float promoRate { get; set; }

        public List<Promos> GetPromos()
        {
            Database db = new Database();
            List<Promos> promos = new List<Promos>();

            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();

                string query = "SELECT p.promo_id, p.promo, p.price, c.rate FROM tbl_promo p " +
                               "JOIN tbl_commission_rate c ON p.commission_rate = c.rate_id;;";
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
                            prmo.promoRate = reader.GetFloat("rate");
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

        public int InsertPromos(string promoName, float promoPrice, int rateID)
        {
            int promoID = -1; // Initialize with a default value
            Database db = new Database();


            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = @"INSERT INTO `tbl_promo` (`promo`, `price`, `commission_rate`) 
                                 VALUES (@PromoName, @PromoPrice, @RateID); 
                         SELECT LAST_INSERT_ID();"; // Retrieve the last inserted ID
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PromoName", promoName);
                cmd.Parameters.AddWithValue("@PromoPrice", promoPrice);
                cmd.Parameters.AddWithValue("@RateID", rateID);


                // Execute the command and retrieve the last inserted ID
                promoID = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return promoID;
        }
    }
}
