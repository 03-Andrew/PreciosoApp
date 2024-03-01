using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;

namespace PreciosoApp.Models
{
    public class Transactions
    {
        public int ID { get; set; }
        public DateTime Date_Time { get; set; }
        public string ClientName { get; set; }
        public string TherapisName { get; set; }
        public string MOP {  get; set; }
        public double Total { get; set; }
        public double TotalCommission { get; set; }
        public string Notes { get; set; }


        /*
         * public List<string> TransactionTypes { get; set; }
        public double TotalService { get; set; }
        public double TotalProduct {  get; set; }
        public bool HasProductsSold { get; set; }
        public List<string> ProductSold { get; set; }
        public bool HasServicesSold { get; set; }
        public List<string> ServicesAvailed {  get; set; }
        public bool HasPromo {  get; set; }
        public List<string> PromoAvailed { get; set; }
        public Transactions() { }
        
        */
        public List<Transactions> GetTransactions()
        {
            Database db = new Database();
            List<Transactions> transactions = new List<Transactions>();

            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = "";
            }
            return transactions;
        }
        
    }

    public class ProductSoldTransactions
    {
        public int Id { get; set; }
        public DateTime Date_Time { get; set; }
        public string ClientName { get; set; }
        public string TherapistName { get; set; }
        public string MOP { get; set; }
        public double Total { get; set; }
        public double TotalCommission { get; set; }
        public string Notes { get; set; }
        public string ProductsSold { get; set; }

        public List<ProductSoldTransactions> GetPTransactions()
        {
            List<ProductSoldTransactions> Ptransactions = new List<ProductSoldTransactions>();

            using (MySqlConnection conn = new MySqlConnection("YourConnectionString"))
            {
                conn.Open();
                string query = "SELECT t.transaction_id, t.transaction_datetime, c.client_name, th.name, mp.mode, t.notes, " +
                               "SUM(ps.quantity * p.product_cost) AS total_price, " +
                               "SUM(ps.quantity * p.commission) AS total_commission, " +
                               "GROUP_CONCAT(CONCAT(p.product_name, ' (Qty: ', ps.quantity, ')')) AS product_list " +
                               "FROM tbl_transaction t " +
                               "JOIN tbl_client c ON t.client_assigned = c.client_id " +
                               "JOIN tbl_therapist th ON t.therapist_assigned = th.therapist_id " +
                               "JOIN tbl_modeofpayment mp ON t.mode_of_payment = mp.mode_id " +
                               "INNER JOIN tbl_products_sold ps ON t.transaction_id = ps.transaction_id " +
                               "INNER JOIN tbl_product p ON ps.product_id = p.product_id " +
                               "GROUP BY t.transaction_id;";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductSoldTransactions pst = new ProductSoldTransactions
                            {
                                Id = reader.GetInt32(0),
                                Date_Time = reader.GetDateTime(1),
                                ClientName = reader.GetString(2),
                                TherapistName = reader.GetString(3),
                                MOP = reader.GetString(4),
                                Total = reader.GetDouble(5),
                                TotalCommission = reader.GetDouble(6),
                                Notes = reader.GetString(7),
                                ProductsSold = reader.IsDBNull(8) ? string.Empty : reader.GetString(8)
                            };

                            Ptransactions.Add(pst);
                        }
                    }
                }
            }
            return Ptransactions;
        }
    }



}
