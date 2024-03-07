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
        Database db = new Database();
        public int ID { get; set; }
        public DateTime Date_Time { get; set; }
        public string ClientName { get; set; }
        public string TherapisName { get; set; }
        public string MOP { get; set; }
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
            
            List<Transactions> transactions = new List<Transactions>();

            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = "select t.transaction_id, t.transaction_datetime, c.client_name, th.name, mp.mode, " +
                    "t.notes from tbl_transaction t left join tbl_client c on t.client_assigned = c.client_id " +
                    "left join tbl_therapist th on t.therapist_assigned = th.therapist_id left join tbl_modeofpayment mp on t.mode_of_payment = mp.mode_id;";

                using(MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        Transactions tr = new Transactions();
                        tr.ID = reader.GetInt32(0);
                        tr.Date_Time = reader.GetDateTime(1);
                        tr.ClientName = reader.GetString(2);
                        tr.TherapisName = reader.GetString(3);
                        tr.MOP = reader.GetString(4);
                        tr.Notes = reader.GetString(5);
                        transactions.Add(tr);
                    }
                }

            }
            return transactions;
        }

        public int InsertTransaction(DateTimeOffset? Date_Time, int ClientID, int TherapistID, int MOP, string Notes)
        {
            int transactionID = -1; // Initialize with a default value


            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = @"INSERT INTO  tbl_transaction  (transaction_datetime, client_assigned, therapist_assigned, mode_of_payment, notes) 
                         VALUES (@Date_Time, @ClientID, @TherapistID, @MOP, @Notes);
                         SELECT LAST_INSERT_ID();"; // Retrieve the last inserted ID
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Date_Time", Date_Time);
                cmd.Parameters.AddWithValue("@ClientID", ClientID);
                cmd.Parameters.AddWithValue("@TherapistID", TherapistID);
                cmd.Parameters.AddWithValue("@MOP", MOP);
                cmd.Parameters.AddWithValue("@Notes", Notes);

                // Execute the command and retrieve the last inserted ID
                transactionID = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return transactionID;
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
        Database db = new Database();

        public List<ProductSoldTransactions> GetPTransactions()
        {
            Func<MySqlDataReader, ProductSoldTransactions> mapRow = reader => new ProductSoldTransactions
            {
                Id = reader.GetInt32(0),
                Date_Time = reader.GetDateTime(1),
                ClientName = reader.GetString(2),
                TherapistName = reader.GetString(3),
                MOP = reader.GetString(4),
                Total = reader.GetDouble(6),
                Comm = reader.GetDouble(7),
            };

            string query = "select * from prod_sold_history;";
            return db.ExecuteQuery(query, mapRow);
        }


        public static implicit operator ProductSoldTransactions(ObservableCollection<ProductSold> v)
        {
            throw new NotImplementedException();
        }
    }

    public class ProductSold
    {
        public int TransactionId { get; set; }
        public string ProdName { get; set; }
        public double ProductCost { get; set; }
        public int Quantity { get; set; }
        public double Commission { get; set; }
        public string ProdSold { get; set; }
        Database db = new Database();

        public List<ProductSold> GetProductsSold()
        {
            Func<MySqlDataReader, ProductSold> mapRow = reader => new ProductSold
            {
                TransactionId = reader.GetInt32(0),
                ProdName = reader.GetString(1),
                ProductCost = reader.GetDouble(2),
                Quantity = reader.GetInt32(3),
                Commission = reader.GetDouble(4)
            };

            string query = "SELECT * FROM prod_sold;";
            return db.ExecuteQuery(query, mapRow);
        }


        public void InsertProductSold(int trnscID, int productID, int qty)
        {
            int transactionID = -1; // Initialize with a default value

            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = @"INSERT INTO tbl_products_sold (transaction_id, product_id, quantity) 
                         VALUES (@trnscID, @productID, @quantity);";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@trnscID", trnscID);
                cmd.Parameters.AddWithValue("@productID", productID);
                cmd.Parameters.AddWithValue("@quantity", qty);
                cmd.ExecuteNonQuery();
            }
        }

    }


    public class Service_Transaction
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string ClientName { get; set; }
        public string Therapist { get; set; }
        public string Mode { get; set; }
        public double Price { get; set; }
        public double Commission { get; set; }
        public string Notes { get; set; }
        Database db = new Database();

        public Service_Transaction() { }

        public List<Service_Transaction> GetService_Transactions()
        {
            Func<MySqlDataReader, Service_Transaction> mapRow = reader => new Service_Transaction
            {
                ID = reader.GetInt32(0),
                Date = reader.GetDateTime(1),
                ClientName = reader.GetString(2),
                Therapist = reader.GetString(3),
                Mode = reader.GetString(4),
                Price = reader.GetDouble(5),
                Commission = reader.GetDouble(6),
                Notes = reader.GetString(7)
            };

            string query = "select * from services_history";

            return db.ExecuteQuery(query, mapRow);

        }
    }


}


/*
 *
 *Trash
 *public List<ProductSold> GetProductsSold2()
        {
            List<ProductSold> Ptransactions = new List<ProductSold>();


            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = "SELECT * FROM prod_sold;";
                //string query = "SELECT transaction_id, GROUP_CONCAT(\" \", product_name, \" (\",quantity,\") \", product_cost*quantity, \" Comm: \", commission, \"\\n\") AS products_sold FROM prod_sold GROUP BY transaction_id ORDER BY transaction_id;";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductSold pst = new ProductSold
                            {
                                TransactionId = reader.GetInt32(0),
                                
                                ProdName = reader.GetString(1),
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


        public List<ProductSoldTransactions> GetPTransactions2()
        {
            List<ProductSoldTransactions> Ptransactions = new List<ProductSoldTransactions>();
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

                using (MySqlCommand cmd = new MySqlCommand(query2, conn))
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
                            };

                            Ptransactions.Add(pst);
                        }
                    }
                }
            }
            return Ptransactions;
        }
 */
            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = @"INSERT INTO tbl_products_sold (transaction_id, product_id, quantity) 
                         VALUES (@trnscID, @productID, @quantity);";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@trnscID", trnscID);
                cmd.Parameters.AddWithValue("@productID", productID);
                cmd.Parameters.AddWithValue("@quantity", qty);
                cmd.ExecuteNonQuery();
            }
        }

    }

    //Appointment table
    public class ServicesUsed
    {
        public int TransactionId { get; set; }
        public int ServiceId { get; set; }
        public int status { get; set; }


        public void insertServiceUsed(int trnscID, int serviceID, int qty)
        {
            int transactionID = -1; // Initialize with a default value

            Database db = new Database();

            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = @"INSERT INTO `tbl_appointment` (`transaction_id`, `service_id`, `quantity`, `appointment_status`) 
                                 VALUES (@trnscID, @serviceID, @quantity, 2);";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@trnscID", trnscID);
                cmd.Parameters.AddWithValue("@quantity", qty);
                cmd.Parameters.AddWithValue("@serviceID", serviceID);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public class PromoTransaction
    {
        public int TransactionId { get; set; }
        public int PromoID { get; set; }

        public void insertPromoTransaction(int trnscID, int promoID, int qty)
        {
            int transactionID = -1; // Initialize with a default value

            Database db = new Database();

            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = @"INSERT INTO `tbl_promo_transaction` (`transaction_id`, `promo_id`, `status`) 
                                 VALUES (@trnscID, @promoID, 2) ";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@trnscID", trnscID);
                cmd.Parameters.AddWithValue("@promoID", promoID);
                cmd.ExecuteNonQuery();
            }

            List<(int serviceID, int quantity)> promoServices = GetPromoServices(promoID);
            var sUsed = new ServicesUsed();

            foreach (var (serviceID, quantity) in promoServices)
            {
                int servQty = quantity * qty;
                sUsed.insertServiceUsed(trnscID, serviceID, servQty);
            }
        }

        private List<(int serviceID, int quantity)> GetPromoServices(int promoID)
        {
            List<(int serviceID, int quantity)> services = new List<(int serviceID, int quantity)>();

            Database db = new Database();

            using (MySqlConnection conn = db.GetCon())
            {
                conn.Open();
                string query = @"SELECT service_id, quantity FROM tbl_promo_services WHERE promo_id = @promoID";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@promoID", promoID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int serviceID = reader.GetInt32("service_id");
                            int quantity = reader.GetInt32("quantity");
                            services.Add((serviceID, quantity));
                        }
                    }
                }
            }

            return services;
        }
    }
}

