using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public double Comm { get; set; }
        public string ProductsSold { get; set; }

        public List<ProductSoldTransactions> GetPTransactions()
        {
            List<ProductSoldTransactions> Ptransactions = new List<ProductSoldTransactions>();
            Database db = new Database();
            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = "SELECT t.transaction_id as ID, t.transaction_datetime as DateTime, c.client_name, th.name as Therapist, mp.mode, t.notes, " +
                               "SUM(ps.quantity * p.product_cost) AS total_price, " +
                               "SUM(ps.quantity * p.commission) AS comm, " +
                               "GROUP_CONCAT(CONCAT(p.product_name, ' (Qty: ', ps.quantity, ')')) AS product_list " +
                               "FROM tbl_transaction t " +
                               "JOIN tbl_client c ON t.client_assigned = c.client_id " +
                               "JOIN tbl_therapist th ON t.therapist_assigned = th.therapist_id " +
                               "JOIN tbl_modeofpayment mp ON t.mode_of_payment = mp.mode_id " +
                               "INNER JOIN tbl_products_sold ps ON t.transaction_id = ps.transaction_id " +
                               "INNER JOIN tbl_product p ON ps.product_id = p.product_id " +
                               "GROUP BY t.transaction_id;";

                string query2 = "select * from prod_sold_history;";

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
                                Total = reader.GetDouble(6),
                                Comm = reader.GetDouble(7),
                                ProductsSold = reader.GetString("product_list")
                            };

                            Ptransactions.Add(pst);
                        }
                    }
                }
            }
            return Ptransactions;
        }

        public static implicit operator ProductSoldTransactions(ObservableCollection<ProductSold> v)
        {
            throw new NotImplementedException();
        }
    }

    public class ProductSold
    {
        public int TransactionId { get; set; }
        public string ProductName { get; set; }
        public double ProductCost { get; set; }
        public int Quantity { get; set; }
        public double Commission { get; set; }

        public List<ProductSold> GetProductsSold()
        {
            List<ProductSold> Ptransactions = new List<ProductSold>();
            Database db = new Database();

            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = "SELECT * FROM prod_sold;";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductSold pst = new ProductSold
                            {
                                TransactionId = reader.GetInt32(0),
                                ProductName = reader.GetString(1),
                                ProductCost = reader.GetDouble(2),
                                Quantity = reader.GetInt32(3),
                                Commission = reader.GetDouble(4)
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
